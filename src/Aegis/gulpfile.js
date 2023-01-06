Skip to content
Search or jump to…
Pull requests
Issues
Codespaces
Marketplace
Explore

@metodiobetsanov
MNB - Software
    /
    chimera
Public
Code
Issues
Pull requests
Actions
Wiki
Security
Insights
Settings
chimera / gulpfile.js
@metodiobetsanov
metodiobetsanov Project architecture and authentication
Latest commit 9214df5 3 weeks ago
History
1 contributor
41 lines(38 sloc)  1.13 KB

const del = require('del');
const gulp = require('gulp');
const npmdist = require('gulp-npm-dist');
const rename = require('gulp-rename');

const paths = {
    base: {
        base: {
            dir: './'
        },
        node: {
            dir: './node_modules'
        }
    },
    src: {
        base: {
            dir: './wwwroot/',
            files: './wwwroot/**/*'
        },
        libs: {
            dir: './wwwroot/libs',
            files: './wwwroot/libs/**/*'
        }
    }
};

gulp.task('copy:libs', function () {
    return gulp
        .src(npmdist(), { base: paths.base.node.dir, sourcemaps: true })
        .pipe(rename(function (path) {
            if (path.dirname.includes('build')) {
                path.dirname = path.dirname.replace(/\/build/, '').replace(/\\build/, '');
            }
            else if (path.dirname.includes('dist')) {
                path.dirname = path.dirname.replace(/\/dist/, '').replace(/\\dist/, '');
            }
        }))
        .pipe(gulp.dest(paths.src.libs.dir, { sourcemaps: '.' }));
});

gulp.task('build', 'copy:libs');