function setTextAnimation(delay, duration, strokeWidth, timingFunction, strokeColor, repeat) {
    let paths = document.querySelectorAll('path');
    let mode = repeat ? 'infinite' : 'forwards'
    for (let i = 0; i < paths.length; i++) {
        const path = paths[i];
        const length = path.getTotalLength();
        path.style['stroke-dashoffset'] = `${length}px`;
        path.style['stroke-dasharray'] = `${length}px`;
        path.style['stroke-width'] = `${strokeWidth}px`;
        path.style['stroke'] = `${strokeColor}`;
        path.style['animation'] = `${duration}s svg-text-anim ${mode} ${timingFunction}`;
        path.style['animation-delay'] = `${i * delay}s`;
    }
}

setTextAnimation(0.1, 2.7, 4, 'linear', '#ffffff', false);

$(function () {
    if ($('.typer')[0]) {
        $('.typer').typed({
            strings: [
                'Hi, my name is Jonathan',
                'I am a Software Developer',
                'I am also a business enthusiast',
                'Best part, I am a hiking nut :-)',
                'Let me know if I can be helpful',
                'Have a great one!'],
            typeSpeed: 50,
            loop: true,
            loopCount: false,
        });
    }
});

const { createApp } = Vue;

const form = createApp({
    methods: {
        async onSubmit() {
            if (Object.keys(this.errors).length > 0) this.errors = {};
            if (this.success !== null) this.success = null;

            const results = await $.post('/api/inquiry/newinquiry', this.form);

            if (!results.success) {
                if (Object.keys(results.errors).includes('userError')) {
                    this.success = results.errors.userError;
                }
                else if (Object.keys(results.errors).includes('serverError')) {
                    this.success = results.errors.serverError;
                }
                this.errors = results.errors;
            }
            else {
                this.success = 'Thank you for your inquiry!';
                this.form = {
                    name: '',
                    email: '',
                    type: '',
                    details: ''
                }
            }
        }
    },
    data() {
        return {
            form: {
                name: '',
                email: '',
                type: '',
                details: ''
            },
            errors: {},
            success: null
        }
    }
}).mount('#inquiry');  