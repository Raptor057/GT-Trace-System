async function playAudio(audio) {
    return new Promise(res => {
        audio.play()
        audio.onended = res
    })
};

const Sfx = new class {
    constructor(success, failure) {
        this.playSuccessSoundAsync = success;
        this.playFailureSoundAsync = failure;
    }

    playEtiUsedAudio = async () => {
        playAudio(new Audio("./sfx/smb_coin.wav"));
    };

    playEtiLoadedAudio = async () => {
        playAudio(new Audio("./sfx/smb_jumpsmall.wav"));
    };

    playEtiUnloadedAudio = async () => {
        playAudio(new Audio("./sfx/smb_kick.wav"));
    };

    playEtiReturnedAudio = async () => {
        playAudio(new Audio("./sfx/smb_stomp.wav"));
    };

    playUnitPackedAudio = async () => {
        playAudio(new Audio("./sfx/smb_coin.wav"));
    };
};

export default Sfx;