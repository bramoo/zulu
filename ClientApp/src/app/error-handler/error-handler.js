"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var PopupErrorHandler = /** @class */ (function () {
    function PopupErrorHandler(injector) {
        this.injector = injector;
        this.defaultHandler = new core_1.ErrorHandler();
    }
    PopupErrorHandler.prototype.handleError = function (error) {
        console.log("[popup-error-handler] yeah boiiii");
        this.defaultHandler.handleError(error);
    };
    return PopupErrorHandler;
}());
exports.PopupErrorHandler = PopupErrorHandler;
//# sourceMappingURL=error-handler.js.map