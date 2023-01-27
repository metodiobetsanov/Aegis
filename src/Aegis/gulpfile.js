const del = require('del');
const gulp = require('gulp');
const filter = require('gulp-filter');
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
      dir: './wwwroot/assets/libs',
      files: './wwwroot/assets/libs/**/*'
    }
  }
};

gulp.task('copy:libs', function () {
  const f = filter(['*.min.js', '**/*.min.js', '*.min.css', '**/*.min.css']);
  return gulp
    .src(npmdist(), { base: paths.base.node.dir, sourcemaps: true })
    .pipe(f)
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

gulp.task('default', gulp.series('copy:libs'));