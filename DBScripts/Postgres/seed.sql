-- Create a Table with the Music Genres
CREATE TABLE Genre
(
    ID SERIAL PRIMARY KEY NOT NULL,
    Style VARCHAR(50) NOT NULL
);

-- Create a table with the Songs and Band Names Associated with them
CREATE TABLE Music
(
    ID SERIAL PRIMARY KEY,
    Band VARCHAR(50) NOT NULL,
    Song VARCHAR(100) NOT NULL,
    Genre INT REFERENCES Genre(ID)
);

-- Insert Some data in both of the Tables generated
INSERT INTO Genre(Style) VALUES
('Rock'),
('Indie'),
('Grunge'),
('Other');

INSERT INTO Music(Band, Song, Genre) VALUES
('Foo Fighters', 'Best Of You', 3),
('Nirvana', 'Come as You Are', 3),
('Alt-J', 'Something Good', 2),
('Kansas', 'Carry on Wayward Son', 1);