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
-- Ajout direct du UserId pour savoir qui est le créateur
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

-- Table des Témoignages
CREATE TABLE Testimonials (
    Id INT IDENTITY PRIMARY KEY,
    AuthorName NVARCHAR(150) NOT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    Rating INT NOT NULL DEFAULT 5,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
