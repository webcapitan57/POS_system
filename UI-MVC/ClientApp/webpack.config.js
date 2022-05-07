const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        site: './src/js/site.js',
        bootstrap_js: './src/js/bootstrap_js.js',
        validation: './src/js/validation.js',
        
        //css
        identity: './src/js/identity.js',
        base: './src/js/base.js',
        setups: './src/js/setUp/setups.js',
        moderate: './src/js/Moderate/moderate.js',
        group: './src/js/group/group.js',
        createTask: './src/js/task/createTask.js',
        task: './src/js/task/task.js',
        admin: './src/js/admin/admin.js',
        
        //admin
        removeMarkedPhoto: './src/js/admin/removeMarkedPhoto.js',
        unmarkPhoto: './src/js/admin/unmarkPhoto.js',
        addSetupToAdmin: './src/js/admin/addSetupToAdmin.js',
        //delivery
        uploadPhoto: './src/js/delivery/uploadPhoto.js',
        locationDelivery: './src/js/delivery/locationDelivery.js',
        //group
        indexGroup: './src/js/group/indexGroup.js',
        addTaskToGroup: './src/js/group/addTaskToGroup.js',
        
        //moderate
        likeModerate: './src/js/Moderate/likeModerate.js',
        moderateDeliveries: './src/js/Moderate/moderateDeliveries.js',
        moderateFlaggedPhotos: './src/js/Moderate/moderateFlaggedPhotos.js',
        moderateSelect: './src/js/Moderate/moderateSelect.js',
        saveFilterProfile: './src/js/Moderate/saveFilterProfile.js',
        deleteAnswer: './src/js/Moderate/deleteAnswer',
        customTags: './src/js/Moderate/customTags',
        moderateDeleteAnswer: './src/js/Moderate/moderateDeleteAnswer.js',
        
        //moderate gallery
        filterGallery: './src/js/Moderate/Gallery/filterGallery.js',
        likeGallery: './src/js/Moderate/Gallery/likeGallery.js', 
        selectGallery: './src/js/Moderate/Gallery/selectGallery.js',
        //moderate grid
        filterCustomTags: './src/js/Moderate/Grid/filterCustomTags.js',
        filterGrid: './src/js/Moderate/Grid/filterGrid.js',
        selectGrid: './src/js/Moderate/Grid/selectGrid.js',
        //moderate slide
        selectSlide: './src/js/Moderate/Slide/selectSlide.js',
        
        //setUp
        addAdminToSetup: './src/js/setUp/addAdminToSetup.js',
        createSetUp: './src/js/setUp/createSetUp.js',
        deleteSetUp: './src/js/setUp/deleteSetUp.js',
        detailsSetUp: './src/js/setUp/detailsSetUp.js',
        editSetUp: './src/js/setUp/editSetUp.js',
        archiveSetUp: './src/js/setUp/archiveSetUp.js',
        uploadSetUpLogo: './src/js/setUp/uploadSetUpLogo.js',
        
        //task
        editTask: './src/js/task/editTask.js',
        deleteTask: './src/js/task/deleteTask.js',
        showAnswers: './src/js/task/showAnswers.js',
        showPhotos: './src/js/task/showPhotos.js',
        uploadedPhotos: './src/js/task/uploadedPhotos.js',
        locationTask: './src/js/task/locationTask.js',
        //teacher
        getTeacher: './src/js/teacher/getTeacher.js',
        switchLogin: './src/js/teacher/login-switch.js',
        register: './src/js/teacher/register.js',
        
        //Vset
        filterProfilesVset: './src/js/Vset/filterProfilesVset.js',
        filterTaskVset: './src/js/Vset/filterTaskVset.js',
        reportVset: './src/js/Vset/reportVset.js',
        selectVset: './src/js/Vset/selectVset.js',
        
        //setup colors
        setupcolors: './src/js/setup-colors.js'
    },
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist')
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {test: /\.css$/, use: [{loader: MiniCssExtractPlugin.loader}, 'css-loader']},
            {test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, use: ['file-loader']},
            {
                test: /\.(woff|woff2)$/, use: [
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 5000,
                        },
                    },
                ]
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/, use: [
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 10000,
                            mimetype: 'application/octet-stream',
                        },
                    },
                ]
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/, use: [
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 10000,
                            mimetype: 'image/svg+xml',
                        },
                    },
                ]
            },
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
    ]
};