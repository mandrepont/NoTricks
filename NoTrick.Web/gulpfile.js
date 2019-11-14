const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const sass = require('gulp-sass');
const minifyCSS = require('gulp-clean-css');
const del = require('del');

const distFolder = './wwwroot/dist/';
const jsFolder = `${distFolder}js/`;
const cssFolder = `${distFolder}css/`;

function processClean() {
    return del(`${distFolder}**`, { force: true });
}

function processDashboardBundle() {
    return gulp
        .src([
            './node_modules/admin-lte/plugins/jquery/jquery.min.js',
            './node_modules/admin-lte/plugins/bootstrap/js/bootstrap.bundle.min.js',
            './node_modules/admin-lte/dist/js/adminlte.min.js',
        ])
        .pipe(concat('bundle.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(jsFolder)); 
}

function processDashboardStyles() {
    return gulp
        .src([
            './node_modules/admin-lte/dist/css/adminlte.min.css',
            './node_modules/admin-lte/plugins/fontawesome-free/css/all.min.css',
        ])
        .pipe(minifyCSS())
        .pipe(concat('bundle.min.css'))
        .pipe(gulp.dest(cssFolder));
}

function processSass() {
    return gulp
        .src('Styles/web.scss')
        .pipe(sass())
        .on('error', sass.logError)
        .pipe(gulp.dest(cssFolder));
}

function processSassMin() {
    return gulp
        .src('Styles/web.scss')
        .pipe(sass())
        .on('error', sass.logError)
        .pipe(minifyCSS())
        .pipe(concat('web.min.css'))
        .pipe(gulp.dest(cssFlder));
}

function processFonts() {
    return gulp
        .src(['./node_modules/admin-lte/plugins/fontawesome-free/webfonts/**'])
        .pipe(gulp.dest(`${distFolder}webfonts/`));
}


const buildAdminStyles = gulp.series(processDashboardStyles); //Add scss when the time comes. 
const buildAdmin = gulp.parallel(buildAdminStyles, processFonts, processDashboardBundle);

gulp.task('clean', processClean);
gulp.task('styles:admin', buildAdminStyles);
gulp.task('sass', processSass);
gulp.task('font', processFonts);
gulp.task('sass:min', processSassMin);
gulp.task('scripts:admin', processDashboardBundle);
gulp.task('build:admin', buildAdmin);