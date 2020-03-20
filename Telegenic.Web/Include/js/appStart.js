﻿import { seriesForms } from "./Admin/seriesForms";

document.onreadystatechange = function documentInitialize(e) {
    if (document.readyState === 'interactive') {
        if (window.location.pathname.startsWith('/Admin/SingleSeries')) {
            let _seriesForms = new seriesForms();
            _seriesForms.bindInit();
        }        
    }
}