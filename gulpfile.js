/// <binding ProjectOpened='watch:tailwind' />
const gulp = require('gulp');
const exec = require('child_process').exec;

gulp.task('watch:tailwind', function (done) {
    const process = exec('npx tailwindcss -i ./wwwroot/css/tailwind.css -o ./wwwroot/css/output.css --watch');

    process.stdout.on('data', function (data) {
        console.log(data);
    });

    process.stderr.on('data', function (data) {
        console.error(data);
    });

    process.on('close', function (code) {
        console.log('Tailwind watcher exited with code ' + code);
        // Mark task as completed
        done()
        // Restart the watcher on exit
        gulp.series('watch:tailwind')();
    });
});