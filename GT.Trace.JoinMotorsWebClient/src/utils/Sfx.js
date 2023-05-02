async function playAudio(audio) {
    return new Promise(res => {
        audio.play()
        audio.onended = res
    })
};

const Sfx = new class {

    constructor() {
        this.counter = 0;
    }

    playSuccessSoundAsync = async function () {
        this.counter = this.counter || 0;
        let sounds = ['mario-ya', 'mario-woohoo', 'mario-yahoo'];
        await playAudio(new Audio(`./sfx/${sounds[this.counter]}.wav`));
        this.counter = ++this.counter % 3;
    };

    playFailureSoundAsync = async function () {
        let sounds = ['invalid', 'mario-doh', 'mario-mamamia'];
        await playAudio(new Audio(`./sfx/${sounds[Math.floor(Math.random() * 3)]}.wav`));
    };

    playPickingSoundAsync = async function () {
        await playAudio(new Audio(`./sfx/boing.wav`));
    };
};

export default Sfx;