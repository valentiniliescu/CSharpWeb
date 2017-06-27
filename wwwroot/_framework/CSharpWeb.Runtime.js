(function() {
    var nextElemId = 0;

    var elementsById = {};

	window['browser.js'] = {
		AppendChild: function (descriptor) {
            var item = JSON.parse(descriptor);
            var parent = item.parentId ? elementsById[item.parentId] : document.body;
            var child = elementsById[item.id];
            parent.appendChild(child);
        },

        CreateElement: function(tagName) {
            var elem = document.createElement(tagName);
            var id = assignUniqueElementId(elem);
            elementsById[id] = elem;
            return id;
        },

        GetInnerText: function(id) {
            var elem = elementsById[id];
            return elem.innerText;
        },

        SetInnerText: function (descriptor) {
            var item = JSON.parse(descriptor);
            var elem = elementsById[item.id];
            elem.innerText = item.innerText;
        },

        AddEventListener: function (descriptor) {
            var item = JSON.parse(descriptor);
            var elem = elementsById[item.id];
            elem.addEventListener(item.eventName, function () {
                InvokeStatic('CSharpWeb.Runtime', 'CSharpWeb', 'HTMLElement', 'ExecuteEventHandler', item.eventHandlerId );
            }, false);
        },

		BeginFetch: function(descriptor) {
			var parsed = JSON.parse(descriptor);
			var url = parsed.url;

            var xhr = new XMLHttpRequest;
            xhr.open("GET", url);
            xhr.onreadystatechange = function xhrOnReadyStateChange (evt) {
                if (xhr.readyState === 4) {
                    InvokeStatic('corlib', 'System.Net.Http', 'HttpClient', 'OnFetchCompleted', JSON.stringify({
                        asyncResultAddress: parsed.asyncResultAddress,
                        response: { statusCode: xhr.status, bodyText: xhr.response }
                    }));
                }
            };

            xhr.send(null);
		}
	};

	function assignUniqueElementId(elem) {
		var id = 'el_' + (++nextElemId);
		elem.id = id;
		return id;
	}
})();

function InvokeStatic(assemblyName, namespace, className, methodName, stringArg) {
	return Module.ccall('JSInterop_CallDotNet', // name of C function
		  'number', // return type
		  ['string', 'string', 'string', 'string', 'string'], // argument types
		  [assemblyName, namespace, className, methodName, stringArg]); // arguments
}

(function () {

    function FetchArrayBuffer(url, onload, onerror) {
        var xhr = new XMLHttpRequest;
        xhr.open("GET", url, true);
        xhr.responseType = "arraybuffer";
        xhr.onload = function xhr_onload() {
            if (xhr.status == 200 || xhr.status == 0 && xhr.response) {
                onload(xhr.response);
            } else {
                onerror();
            }
        };
        xhr.onerror = onerror;
        xhr.send(null);
    }

    function StartApplication(entryPoint, referenceAssemblies) {
        var preloadAssemblies = [entryPoint].concat(referenceAssemblies).map(function (assemblyName) {
            return { assemblyName: assemblyName, url: '_bin/' + assemblyName };
        });
        preloadAssemblies.push({ assemblyName: 'CSharpWeb.Runtime.dll', url: '_framework/CSharpWeb.Runtime.dll' });

        window.Module = {
            wasmBinaryFile: '_framework/wasm/dna.wasm',
            asmjsCodeFile: '_framework/asmjs/dna.asm.js',
            arguments: [entryPoint],
            preRun: function () {
                // Preload corlib.dll and other assemblies
                Module.readAsync = FetchArrayBuffer;
                Module.FS_createPreloadedFile('/', 'corlib.dll', '_framework/corlib.dll', true, false);
                preloadAssemblies.forEach(function (assemblyInfo) {
                    Module.FS_createPreloadedFile('/', assemblyInfo.assemblyName, assemblyInfo.url, true, false);
                });
            },
            postRun: function () {
                InvokeStatic('CSharpWeb.Runtime', 'CSharpWeb.Runtime.Interop', 'Startup', 'EnsureAssembliesLoaded', JSON.stringify(
                    preloadAssemblies.map(function (assemblyInfo) {
                        var name = assemblyInfo.assemblyName;
                        var isDll = name.substring(name.length - 4) === '.dll';
                        return isDll ? name.substring(0, name.length - 4) : null;
                    })
                ));
            }
        };

        var browserSupportsNativeWebAssembly = typeof WebAssembly !== 'undefined' && WebAssembly.validate;
        var dnaJsUrl = browserSupportsNativeWebAssembly
            ? '_framework/wasm/dna.js'
            : '_framework/asmjs/dna.js';

        if (!browserSupportsNativeWebAssembly) {
            // In the asmjs case, the initial memory structure is in a separate file we need to download
            var meminitXHR = Module['memoryInitializerRequest'] = new XMLHttpRequest();
            meminitXHR.open('GET', '_framework/asmjs/dna.js.mem');
            meminitXHR.responseType = 'arraybuffer';
            meminitXHR.send(null);
        }

        // Can't load dna.js until Module is configured
        document.write("<script defer src=\"_framework/emsdk-browser.js\"></script>");
        document.write("<script defer src=\"" + dnaJsUrl + "\"></script>");
    }

    // Find own <script> tag
    var allScriptElems = document.getElementsByTagName('script');
    var thisScriptElem = allScriptElems[allScriptElems.length - 1];

    // Read attributes from own <script> tag and then start the application
    var entrypoint = thisScriptElem.getAttribute('main');
    var referenceAssembliesCombined = thisScriptElem.getAttribute('references');
    var referenceAssemblies = referenceAssembliesCombined ? referenceAssembliesCombined.split(',').map(function (s) { return s.trim() }) : [];
    StartApplication(entrypoint, referenceAssemblies);
})();
