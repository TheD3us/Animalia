-- ========================================
-- SCRIPT COMPLET : CRÉATION + PEUPLEMENT
-- Base de données AnimaliaDb
-- ========================================

-- Supprime la base si elle existe déjà
IF DB_ID('AnimaliaDb') IS NOT NULL
BEGIN
    ALTER DATABASE AnimaliaDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AnimaliaDb;
END
GO

-- Création de la base
CREATE DATABASE AnimaliaDb;
GO
USE AnimaliaDb;
GO

PRINT '=== Création de la structure de la base ===';

-- Table des utilisateurs
CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Prenom NVARCHAR(100),
    Nom NVARCHAR(100),
    IsAdmin BIT NOT NULL DEFAULT 0
);

-- Table des ProgramModels (packs/offres)
CREATE TABLE ProgramModels (
    Id INT IDENTITY PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Summary NVARCHAR(MAX),
    Difficulty NVARCHAR(50),
    Price DECIMAL(10,2) NOT NULL DEFAULT 0,
    ImageUrl NVARCHAR(255)
);

-- Table des Trainings (entraînements)
CREATE TABLE Trainings (
    Id INT IDENTITY PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    DurationMinutes INT,
    Equipment NVARCHAR(100),
    Level NVARCHAR(50),
    UserId INT NOT NULL,
    CONSTRAINT FK_Trainings_User FOREIGN KEY (UserId)
        REFERENCES Users(Id) ON DELETE CASCADE
);

-- Table de liaison ProgramModel <-> Training
CREATE TABLE ProgramTrainings (
    ProgramModelId INT NOT NULL,
    TrainingId INT NOT NULL,
    PRIMARY KEY (ProgramModelId, TrainingId),
    CONSTRAINT FK_PT_Program FOREIGN KEY (ProgramModelId)
        REFERENCES ProgramModels(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PT_Training FOREIGN KEY (TrainingId)
        REFERENCES Trainings(Id) ON DELETE CASCADE
);

-- Table des Events
CREATE TABLE Events (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    DateTime DATETIME2 NOT NULL,
    Location NVARCHAR(200),
    Notes NVARCHAR(MAX),
    MaxParticipants INT,
    CONSTRAINT FK_Events_User FOREIGN KEY (UserId)
        REFERENCES Users(Id) ON DELETE CASCADE
);

-- Table de liaison Event <-> Users
CREATE TABLE EventUser (
    EventId INT NOT NULL,
    UserId INT NOT NULL,
    PRIMARY KEY (EventId, UserId),
    CONSTRAINT FK_EU_Event FOREIGN KEY (EventId)
        REFERENCES Events(Id),
    CONSTRAINT FK_EU_User FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);

-- Table des Témoignages
CREATE TABLE Testimonials (
    Id INT IDENTITY PRIMARY KEY,
    AuthorName NVARCHAR(150) NOT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    Rating INT NOT NULL DEFAULT 5,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

PRINT '✓ Structure de la base créée';
GO

-- ========================================
-- PARTIE 2 : PEUPLEMENT DES DONNÉES
-- ========================================

PRINT '';
PRINT '=== Peuplement de la base de données ===';

-- ========================================
-- 1. CRÉATION DES UTILISATEURS
-- ========================================
PRINT '=== Création des utilisateurs ===';

-- Utilisateurs Admin
INSERT INTO Users (Email, Password, Prenom, Nom, IsAdmin) VALUES
('admin@animalia.com', 'Admin123!', 'Admin', 'Principal', 1),
('estelle.martin@animalia.com', 'Admin123!', 'Estelle', 'Martin', 1);

-- Utilisateurs normaux
INSERT INTO Users (Email, Password, Prenom, Nom, IsAdmin) VALUES
('jean.dupont@gmail.com', 'User123!', 'Jean', 'Dupont', 0),
('marie.bernard@outlook.com', 'User123!', 'Marie', 'Bernard', 0),
('lucas.petit@yahoo.fr', 'User123!', 'Lucas', 'Petit', 0),
('sophie.durand@gmail.com', 'User123!', 'Sophie', 'Durand', 0),
('thomas.leroy@outlook.com', 'User123!', 'Thomas', 'Leroy', 0),
('camille.moreau@gmail.com', 'User123!', 'Camille', 'Moreau', 0);

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' utilisateurs créés';
GO

-- ========================================
-- 2. CRÉATION DES TRAININGS
-- ========================================
PRINT '=== Création des trainings ===';

DECLARE @AdminId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'admin@animalia.com');
DECLARE @EstelleId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'estelle.martin@animalia.com');
DECLARE @JeanId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'jean.dupont@gmail.com');
DECLARE @MarieId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'marie.bernard@outlook.com');

-- Trainings créés par l'admin principal
INSERT INTO Trainings (Title, Description, DurationMinutes, Equipment, Level, UserId) VALUES
('Cardio Canin Intense', 'Séance de cardio dynamique avec votre chien pour améliorer endurance et complicité.', 45, 'Laisse, harnais', 'Intermédiaire', @AdminId),
('Agility Débutant', 'Initiation à l''agility : parcours d''obstacles simples pour développer agilité et obéissance.', 60, 'Cônes, haies basses, tunnel', 'Débutant', @AdminId),
('Obéissance Avancée', 'Perfectionnement des ordres de base et apprentissage de commandes complexes.', 50, 'Clicker, friandises', 'Avancé', @AdminId),
('Yoga Doga', 'Séance de relaxation et étirements partagés avec votre compagnon à quatre pattes.', 30, 'Tapis de yoga', 'Débutant', @AdminId),
('Course Nature', 'Trail running avec votre chien sur sentiers naturels pour les amateurs de grand air.', 90, 'Laisse de jogging, gourde', 'Intermédiaire', @AdminId);

-- Trainings créés par Estelle (admin)
INSERT INTO Trainings (Title, Description, DurationMinutes, Equipment, Level, UserId) VALUES
('Natation Canine', 'Séance aquatique pour renforcer muscles et articulations en douceur.', 40, 'Gilet de flottaison', 'Intermédiaire', @EstelleId),
('Tricks & Fun', 'Apprentissage de tours amusants : faire le beau, rouler, faire la révérence.', 45, 'Friandises, clicker', 'Débutant', @EstelleId),
('Crossfit Canin Extrême', 'Entraînement de haute intensité pour chiens sportifs et leurs maîtres endurants.', 60, 'Poids, obstacles', 'Extrême', @EstelleId);

-- Trainings créés par Jean (utilisateur normal)
INSERT INTO Trainings (Title, Description, DurationMinutes, Equipment, Level, UserId) VALUES
('Marche en Ville', 'Apprendre à votre chien à marcher calmement en environnement urbain.', 40, 'Laisse courte', 'Débutant', @JeanId),
('Socialisation Chiot', 'Rencontres encadrées pour socialiser les chiots avec leurs congénères.', 50, 'Aucun', 'Débutant', @JeanId);

-- Trainings créés par Marie (utilisateur normal)
INSERT INTO Trainings (Title, Description, DurationMinutes, Equipment, Level, UserId) VALUES
('Massage Canin', 'Techniques de massage pour détendre et soulager votre animal.', 30, 'Huile de massage', 'Débutant', @MarieId),
('Randonnée Montagne', 'Excursion en montagne adaptée aux chiens aventuriers.', 180, 'Sac à dos, gamelle pliable', 'Avancé', @MarieId);

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' trainings créés';
GO

-- ========================================
-- 3. CRÉATION DES PROGRAMMES
-- ========================================
PRINT '=== Création des programmes ===';

INSERT INTO ProgramModels (Title, Summary, Difficulty, Price, ImageUrl) VALUES
('Pack Découverte', 'Programme idéal pour débuter avec votre chien : obéissance de base et socialisation.', 'Débutant', 49.99, '/images/programs/decouverte.jpg'),
('Pack Sport Canin', 'Pour les sportifs : cardio, agility et course nature pour vous et votre chien.', 'Intermédiaire', 79.99, '/images/programs/sport.jpg'),
('Pack Bien-être', 'Programme relaxation : yoga, massage et natation pour une harmonie totale.', 'Débutant', 59.99, '/images/programs/bien-etre.jpg'),
('Pack Expert', 'Formation complète pour les maîtres exigeants : obéissance avancée et tricks.', 'Avancé', 99.99, '/images/programs/expert.jpg'),
('Pack Warrior', 'Le programme ultime pour les plus téméraires : crossfit extrême et randonnées intenses.', 'Extrême', 129.99, '/images/programs/warrior.jpg'),
('Pack Urbain', 'Spécialement conçu pour la vie en ville : marche urbaine et socialisation.', 'Débutant', 39.99, '/images/programs/urbain.jpg');

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' programmes créés';
GO

-- ========================================
-- 4. LIAISON PROGRAMMES <-> TRAININGS
-- ========================================
PRINT '=== Création des liaisons programmes-trainings ===';

DECLARE @PackDecouverte INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Découverte');
DECLARE @PackSport INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Sport Canin');
DECLARE @PackBienEtre INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Bien-être');
DECLARE @PackExpert INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Expert');
DECLARE @PackWarrior INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Warrior');
DECLARE @PackUrbain INT = (SELECT Id FROM ProgramModels WHERE Title = 'Pack Urbain');

DECLARE @AgilityDebutant INT = (SELECT Id FROM Trainings WHERE Title = 'Agility Débutant');
DECLARE @SocialisationChiot INT = (SELECT Id FROM Trainings WHERE Title = 'Socialisation Chiot');
DECLARE @CardioCanin INT = (SELECT Id FROM Trainings WHERE Title = 'Cardio Canin Intense');
DECLARE @CourseNature INT = (SELECT Id FROM Trainings WHERE Title = 'Course Nature');
DECLARE @YogaDoga INT = (SELECT Id FROM Trainings WHERE Title = 'Yoga Doga');
DECLARE @NatationCanine INT = (SELECT Id FROM Trainings WHERE Title = 'Natation Canine');
DECLARE @MassageCanin INT = (SELECT Id FROM Trainings WHERE Title = 'Massage Canin');
DECLARE @ObéissanceAvancée INT = (SELECT Id FROM Trainings WHERE Title = 'Obéissance Avancée');
DECLARE @TricksFun INT = (SELECT Id FROM Trainings WHERE Title = 'Tricks & Fun');
DECLARE @CrossfitExtreme INT = (SELECT Id FROM Trainings WHERE Title = 'Crossfit Canin Extrême');
DECLARE @RandoMontagne INT = (SELECT Id FROM Trainings WHERE Title = 'Randonnée Montagne');
DECLARE @MarcheVille INT = (SELECT Id FROM Trainings WHERE Title = 'Marche en Ville');

-- Pack Découverte : Trainings débutants
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackDecouverte, @AgilityDebutant),
(@PackDecouverte, @SocialisationChiot),
(@PackDecouverte, @YogaDoga);

-- Pack Sport Canin : Trainings cardio et sport
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackSport, @CardioCanin),
(@PackSport, @AgilityDebutant),
(@PackSport, @CourseNature),
(@PackSport, @NatationCanine);

-- Pack Bien-être : Trainings relaxation
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackBienEtre, @YogaDoga),
(@PackBienEtre, @NatationCanine),
(@PackBienEtre, @MassageCanin);

-- Pack Expert : Trainings avancés
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackExpert, @ObéissanceAvancée),
(@PackExpert, @TricksFun),
(@PackExpert, @AgilityDebutant),
(@PackExpert, @CardioCanin);

-- Pack Warrior : Trainings extrêmes
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackWarrior, @CrossfitExtreme),
(@PackWarrior, @RandoMontagne),
(@PackWarrior, @CourseNature);

-- Pack Urbain : Trainings ville
INSERT INTO ProgramTrainings (ProgramModelId, TrainingId) VALUES
(@PackUrbain, @MarcheVille),
(@PackUrbain, @SocialisationChiot);

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' liaisons programmes-trainings créées';
GO

-- ========================================
-- 5. CRÉATION DES ÉVÉNEMENTS
-- ========================================
PRINT '=== Création des événements ===';

DECLARE @AdminId2 INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'admin@animalia.com');
DECLARE @EstelleId2 INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'estelle.martin@animalia.com');
DECLARE @JeanId2 INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'jean.dupont@gmail.com');
DECLARE @MarieId2 INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'marie.bernard@outlook.com');
DECLARE @LucasId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'lucas.petit@yahoo.fr');
DECLARE @SophieId INT = (SELECT TOP 1 Id FROM Users WHERE Email = 'sophie.durand@gmail.com');

-- Événements organisés par l'admin
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@AdminId2, 'Journée Portes Ouvertes Animalia', DATEADD(DAY, 7, GETDATE()), 'Centre Animalia, 123 Rue du Sport', 'Venez découvrir nos installations et rencontrer nos entraîneurs. Démonstrations d''agility et de tricks.', 50),
(@AdminId2, 'Compétition d''Agility Amateur', DATEADD(DAY, 21, GETDATE()), 'Parc Municipal des Sports', 'Compétition amicale ouverte à tous les niveaux. Inscription gratuite, prix à gagner !', 30),
(@AdminId2, 'Stage Intensif Weekend', DATEADD(DAY, 14, GETDATE()), 'Centre Animalia', 'Stage de 2 jours : obéissance, agility et tricks. Niveau intermédiaire requis.', 15);

-- Événements organisés par Estelle (admin)
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@EstelleId2, 'Sortie Plage Canine', DATEADD(DAY, 10, GETDATE()), 'Plage des Sables d''Or', 'Journée détente avec baignade surveillée et jeux aquatiques pour nos compagnons.', 25),
(@EstelleId2, 'Atelier Nutrition Canine', DATEADD(DAY, 5, GETDATE()), 'Salle de Conférence Animalia', 'Conférence sur l''alimentation équilibrée pour nos chiens avec une nutritionniste vétérinaire.', 40);

-- Événements organisés par Jean (utilisateur)
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@JeanId2, 'Balade Urbaine Collective', DATEADD(DAY, 3, GETDATE()), 'Place de la Mairie', 'Balade éducative en ville pour travailler la sociabilisation. Tous niveaux bienvenus.', 20),
(@JeanId2, 'Café Toutous', DATEADD(DAY, 12, GETDATE()), 'Café des Amis, Centre-ville', 'Rencontre conviviale entre maîtres et chiens autour d''un café. Entrée libre.', NULL);

-- Événements organisés par Marie (utilisateur)
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@MarieId2, 'Randonnée Forêt de Fontainebleau', DATEADD(DAY, 15, GETDATE()), 'Parking de la Forêt', 'Randonnée de 10km adaptée aux chiens. Prévoir eau et collation.', 18),
(@MarieId2, 'Séance Massage Découverte', DATEADD(DAY, 8, GETDATE()), 'Centre Bien-Être Animalia', 'Atelier pratique pour apprendre les bases du massage canin.', 12);

-- Événements organisés par Lucas (utilisateur)
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@LucasId, 'Canicross Débutant', DATEADD(DAY, 18, GETDATE()), 'Stade Municipal', 'Initiation au canicross : course avec son chien. Matériel fourni.', 15),
(@LucasId, 'Soirée Ciné-Débat Canin', DATEADD(DAY, 25, GETDATE()), 'Cinéma Le Palace', 'Projection d''un documentaire sur l''éducation positive suivie d''un débat.', 50);

-- Événements organisés par Sophie (utilisateur)
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@SophieId, 'Atelier Jeux Olfactifs', DATEADD(DAY, 6, GETDATE()), 'Parc Canin Municipal', 'Découverte des jeux de flair pour stimuler votre chien mentalement.', 12);

-- Événement passé pour tests
INSERT INTO Events (UserId, Title, DateTime, Location, Notes, MaxParticipants) VALUES
(@AdminId2, 'Concours de Déguisements Halloween', DATEADD(DAY, -10, GETDATE()), 'Centre Animalia', 'Événement passé - Super soirée déguisée avec nos toutous !', 35);

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' événements créés';
GO

-- ========================================
-- 6. INSCRIPTION DES UTILISATEURS AUX ÉVÉNEMENTS
-- ========================================
PRINT '=== Inscription des utilisateurs aux événements ===';

DECLARE @EventPortesOuvertes INT = (SELECT Id FROM Events WHERE Title = 'Journée Portes Ouvertes Animalia');
DECLARE @EventCompetition INT = (SELECT Id FROM Events WHERE Title = 'Compétition d''Agility Amateur');
DECLARE @EventPlage INT = (SELECT Id FROM Events WHERE Title = 'Sortie Plage Canine');
DECLARE @EventBalade INT = (SELECT Id FROM Events WHERE Title = 'Balade Urbaine Collective');

DECLARE @User1 INT = (SELECT Id FROM Users WHERE Email = 'jean.dupont@gmail.com');
DECLARE @User2 INT = (SELECT Id FROM Users WHERE Email = 'marie.bernard@outlook.com');
DECLARE @User3 INT = (SELECT Id FROM Users WHERE Email = 'lucas.petit@yahoo.fr');
DECLARE @User4 INT = (SELECT Id FROM Users WHERE Email = 'sophie.durand@gmail.com');
DECLARE @User5 INT = (SELECT Id FROM Users WHERE Email = 'thomas.leroy@outlook.com');
DECLARE @User6 INT = (SELECT Id FROM Users WHERE Email = 'camille.moreau@gmail.com');

-- Inscriptions aux événements
INSERT INTO EventUser (EventId, UserId) VALUES
-- Journée Portes Ouvertes : beaucoup d'inscrits
(@EventPortesOuvertes, @User1),
(@EventPortesOuvertes, @User2),
(@EventPortesOuvertes, @User3),
(@EventPortesOuvertes, @User4),
(@EventPortesOuvertes, @User5),
-- Compétition : quelques inscrits
(@EventCompetition, @User1),
(@EventCompetition, @User3),
(@EventCompetition, @User6),
-- Sortie Plage : inscrits variés
(@EventPlage, @User2),
(@EventPlage, @User4),
(@EventPlage, @User5),
(@EventPlage, @User6),
-- Balade Urbaine : peu d'inscrits
(@EventBalade, @User2),
(@EventBalade, @User3);

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' inscriptions aux événements créées';
GO

-- ========================================
-- 7. CRÉATION DES TÉMOIGNAGES
-- ========================================
PRINT '=== Création des témoignages ===';

INSERT INTO Testimonials (AuthorName, Text, Rating, CreatedAt) VALUES
('Jean Dupont', 'Excellente expérience ! Mon chien Max a fait d''énormes progrès en obéissance grâce aux cours d''Animalia. Les entraîneurs sont très professionnels.', 5, DATEADD(DAY, -30, GETDATE())),
('Marie Bernard', 'Le Pack Bien-être est parfait ! Les séances de yoga doga nous ont vraiment rapprochés, ma chienne Luna et moi. Je recommande vivement.', 5, DATEADD(DAY, -25, GETDATE())),
('Lucas Petit', 'Super ambiance lors de la compétition d''agility ! Mon border collie Rex s''est éclaté. Vivement la prochaine !', 5, DATEADD(DAY, -20, GETDATE())),
('Sophie Durand', 'Les cours sont bien organisés mais j''aurais aimé plus de créneaux le weekend. Sinon très satisfaite du contenu.', 4, DATEADD(DAY, -15, GETDATE())),
('Thomas Leroy', 'Mon golden retriever Buddy adore la natation canine ! C''est devenu son activité préférée. Merci Animalia !', 5, DATEADD(DAY, -10, GETDATE())),
('Camille Moreau', 'Le Pack Sport Canin est intense mais tellement gratifiant ! Ma malinoise Bella est beaucoup plus calme à la maison depuis qu''on fait du crossfit ensemble.', 5, DATEADD(DAY, -5, GETDATE())),
('Pierre Laurent', 'Bon rapport qualité-prix. Les installations sont propres et bien entretenues.', 4, DATEADD(DAY, -12, GETDATE())),
('Isabelle Dubois', 'Les ateliers tricks sont géniaux ! Mon teckel Oscar sait maintenant faire le beau et rouler. Les autres participants étaient adorables.', 5, DATEADD(DAY, -18, GETDATE()));

PRINT '✓ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' témoignages créés';
GO

-- ========================================
-- 8. STATISTIQUES FINALES
-- ========================================
PRINT '';
PRINT '========================================';
PRINT 'STATISTIQUES DE LA BASE DE DONNÉES';
PRINT '========================================';

SELECT 
    'Utilisateurs' AS Type,
    COUNT(*) AS Total,
    SUM(CASE WHEN IsAdmin = 1 THEN 1 ELSE 0 END) AS Admins,
    SUM(CASE WHEN IsAdmin = 0 THEN 1 ELSE 0 END) AS Utilisateurs
FROM Users;

SELECT 
    'Trainings' AS Type,
    COUNT(*) AS Total
FROM Trainings;

SELECT 
    'Programmes' AS Type,
    COUNT(*) AS Total
FROM ProgramModels;

SELECT 
    'Événements' AS Type,
    COUNT(*) AS Total,
    SUM(CASE WHEN DateTime > GETDATE() THEN 1 ELSE 0 END) AS [À venir],
    SUM(CASE WHEN DateTime <= GETDATE() THEN 1 ELSE 0 END) AS Passés
FROM Events;

SELECT 
    'Témoignages' AS Type,
    COUNT(*) AS Total,
    CAST(AVG(CAST(Rating AS FLOAT)) AS DECIMAL(3,2)) AS [Note moyenne]
FROM Testimonials;

PRINT '';
PRINT '========================================';
PRINT '✓ BASE DE DONNÉES CRÉÉE ET PEUPLÉE !';
PRINT '========================================';
PRINT '';
PRINT 'COMPTES ADMIN :';
PRINT '  - admin@animalia.com / Admin123!';
PRINT '  - estelle.martin@animalia.com / Admin123!';
PRINT '';
PRINT 'COMPTES UTILISATEURS :';
PRINT '  - jean.dupont@gmail.com / User123!';
PRINT '  - marie.bernard@outlook.com / User123!';
PRINT '  - lucas.petit@yahoo.fr / User123!';
PRINT '  - sophie.durand@gmail.com / User123!';
PRINT '  - thomas.leroy@outlook.com / User123!';
PRINT '  - camille.moreau@gmail.com / User123!';
GO
