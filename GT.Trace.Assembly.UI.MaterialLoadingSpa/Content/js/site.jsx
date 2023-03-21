const fitText = function (selector) {
    let spans = [], span = null, rect1 = span, rect2 = span, oldFontSize = 0.0, newFontSize = 0.0, w = 0.0, h = 0.0;
    spans = [...document.querySelectorAll(selector)];
    for (let i = 0; i < spans.length; i++) {
        span = spans[i];
        rect1 = span.getBoundingClientRect();
        rect2 = span.parentElement.getBoundingClientRect();
        oldFontSize = parseFloat(window.getComputedStyle(span, null).getPropertyValue('font-size'));
        w = rect2.width / rect1.width;
        h = rect2.height / rect1.height;
        newFontSize = oldFontSize * (h > w ? w : h);
        span.parentElement.style.fontSize = String(newFontSize) + 'px';
        span.parentElement.style.lineHeight = String(newFontSize * 1.3) + 'px';
    }
};

//let input = '';
//const handleDocumentKeyPress = (e) => {
//    e.preventDefault();
//    e.stopPropagation();
//    if (e.keyCode == 13) {
//        input = input.replace(']C1', '');
//        console.log('input', input);
//        if (input) {
//            alert(input);
//            //const returnModeIsEnabledCheckbox = document.querySelector('#return-mode-is-enabled-checkbox');
//            if (returnModeIsEnabledCheckbox.checked) {
//                if (confirm(`ETI#${input} será retornada. Presióne OK para continuar...`)) {
//                    //returnEti(params['line'], input);
//                }
//            }
//            else {
//                //useEti(params['line'], input, (data) => { });
//            }
//            input = '';
//        }
//    }
//    else {
//        input += e.key;
//    }
//    return false;
//};