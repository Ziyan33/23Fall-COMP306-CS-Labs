// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteMovie(movieId, s3Key) {
    if (!confirm('Are you sure you want to delete this movie?')) return;

    fetch('/Movie/DeleteMovie', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value // For CSRF protection
        },
        body: JSON.stringify({ movieId: movieId, S3Key: s3Key })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Movie deleted successfully');
                // Remove the movie element from the DOM
                document.getElementById(`movie-${movieId}`).remove();
            } else {
                alert('Failed to delete the movie');
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
function downloadMovie(movieId, s3Key) {
    window.location.href = `/Movie/DownloadMovie?movieId=${movieId}&s3Key=${encodeURIComponent(s3Key)}`;
}
