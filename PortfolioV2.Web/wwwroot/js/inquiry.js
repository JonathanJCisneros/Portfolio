const { createApp } = Vue;

const form = createApp({
    methods: {
        async onSubmit() {
            if (Object.keys(this.errors).length > 0) this.errors = {};
            if (this.success !== null) this.success = null;

            const results = await fetch('/api/inquiry/newinquiry', {
                method: "POST",
                body: JSON.stringify(this.form),
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            })
                .then(res => res.json())
                .catch(err => console.log(err));                ;

            !results.success ? this.errors = results.errors : this.success = "Thank you for your inquiry!";
        }
    },
    data() {
        return {
            form: {
                name: '',
                email: '',
                inquiryType: '',
                details: ''
            },
            errors: {},
            success: null
        }
    }
}).mount('#inquiry');  