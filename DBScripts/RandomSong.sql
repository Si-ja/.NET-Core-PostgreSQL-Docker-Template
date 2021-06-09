SELECT music.band, music.song, genre.style
FROM music
INNER JOIN genre ON music.genre = genre.id
ORDER BY random()
LIMIT 1;