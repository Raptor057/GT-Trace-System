/**
 * https://animate.style/
 */
const animationDurationPropertyName = "--animate-duration";
const animationEndEventName = "animationend";

const Animator = new class {
    getElement = (pointOfUseComponent) =>
        document.querySelector(`#${pointOfUseComponent.code}_${pointOfUseComponent.component.number}`);

    async animateElement (element, animationClassName, durationInSeconds = null) {
        return new Promise((resolve, reject) => {
            if (!element.classList.contains(animationClassName)) {
                if (durationInSeconds) {
                    element.style.setProperty(animationDurationPropertyName, `${durationInSeconds}s`);
                }
                element.classList.add(animationClassName);
                let onAnimationEnd;
                onAnimationEnd = (event) => {
                    event.stopPropagation();
                    element.classList.remove(animationClassName);
                    element.removeEventListener(animationEndEventName, onAnimationEnd);
                    if (element.hasOwnProperty(animationDurationPropertyName)) { 
                        delete element[animationDurationPropertyName];
                    }                    
                    resolve("Animation ended");
                };
                element.addEventListener(animationEndEventName, onAnimationEnd, { once: true });
            }
            else {
                reject("Elemento ya esta en animación");
            }
        });
    };

    animate = (pointOfUseComponent, animationName) =>
        this.animateElement(this.getElement(pointOfUseComponent), `animate__${animationName}`);

    tada = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "tada");

    headShake = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "headShake");

    pulse = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "pulse");

    heartBeat = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "heartBeat");

    flash = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "flash");

    bounceIn = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "bounceIn");

    bounceOutDown = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "bounceOutDown");

    flipInX = (pointOfUseComponent) =>
        this.animate(pointOfUseComponent, "flipInX");
};

export default Animator;