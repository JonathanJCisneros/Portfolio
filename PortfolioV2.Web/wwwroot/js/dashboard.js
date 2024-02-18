const { createApp } = Vue;

const form = createApp({
    async mounted() {
        await this.renderData();
    },
    methods: {
        async renderData() {
            if (this.message) this.message = null; 
            this.inquiries = await $.get('/api/inquiry/getinquiries');
            if (this.loading) setTimeout(() => this.loading = false, 800);
        },
        async resolve(id) {
            const results = await $.get('/api/inquiry/resolve', { id: id });
            if (results.success) {
                await this.renderData();
            }
            else {
                this.message = results.errors['serverError'];
            }
        },
        async deleteOne(id) {
            const results = await $.get('/api/inquiry/delete', { id: id });
            if (results.success) {
                await this.renderData();
            }
            else {
                this.message = results.errors['serverError'];
            }
        }
    },
    data() {
        return {
            inquiries: [],
            message: null,
            loading: true
        }
    }
}).mount('#dashboard');  