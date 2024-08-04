function setCookie(cname, cvalue, exdays) {
    const date = new Date();

    date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));

    const expires = 'expires=' + date.toUTCString();
    document.cookie = `${encodeURIComponent(cname)}=${encodeURIComponent(cvalue)};${expires};path=/`;
}

function getCookie(cname) {
    let name = cname + '=';
    let ca = document.cookie.split(';');

    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];

        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }

        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }

    return '';
}

(async function checkCookie() {
    let name = 'user_ip';
    const cookie = getCookie(name);

    if (cookie === '') {
        const results = await fetch('https://api.my-ip.io/v2/ip.json')
            .then(data => data.json());

        setCookie(name, JSON.stringify(results), 1);
    }
})();