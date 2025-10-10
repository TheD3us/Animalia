USE AnimaliaDb;

INSERT INTO Users (Email, Password, Prenom, Nom, IsAdmin)
VALUES 
('educateur@animalia.com', 'admin123', 'Lucie', 'Canin', 1),
('maitre1@animalia.com', 'chien123', 'Paul', 'Dupont', 0),
('maitre2@animalia.com', 'chien456', 'Marie', 'Leroy', 0);


INSERT INTO ProgramModels (Title, Summary, Difficulty, Price, ImageUrl)
VALUES
('Initiation Agility', 'Programme pour découvrir l’agility avec son chien, parcours simples et ludiques.', 'Facile', 15.00, 'agility.jpg'),
('Cardio Canicross', 'Course en duo maître-chien pour améliorer endurance et complicité.', 'Moyen', 25.00, 'canicross.jpg'),
('Renforcement Obéissance & Fitness', 'Séances combinant exercices de musculation légère pour le maître et obéissance pour le chien.', 'Difficile', 30.00, 'fitnesschien.jpg');


INSERT INTO Trainings (Title, DurationMinutes, Equipment, Level)
VALUES
('Parcours Agility Basique', 20, 'Tunnel, haies basses', 'Facile'),
('Séance Canicross 3km', 30, 'Longe, harnais', 'Moyen'),
('Exercices Obéissance + Squats', 25, 'Aucun', 'Moyen'),
('Parcours Agility Avancé', 40, 'Slalom, haies hautes', 'Difficile');

-- Initiation Agility
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES (1, 1);

-- Cardio Canicross
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES (2, 2);

-- Renforcement Obéissance & Fitness
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES (3, 3), (3, 4);

INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants)
VALUES
(1, 'Découverte Agility en groupe', '2025-10-15 14:00:00', 'Club Canin de Segré', 'Amenez friandises et jouets', 10),
(2, 'Sortie Canicross en forêt', '2025-10-20 09:30:00', 'Forêt de Pouancé', 'Prévoir eau pour vous et votre chien', 15);

INSERT INTO Testimonials (AuthorName, Text, Rating)
VALUES
('Camille', 'Mon chien a adoré le parcours agility, et moi aussi !', 5),
('Thomas', 'Le canicross est intense mais super pour renforcer le lien.', 4),
('Julie', 'Les exercices combinés maître-chien sont très motivants.', 5);