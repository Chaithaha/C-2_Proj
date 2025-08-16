-- CreativeColab Database Setup Script
-- Import this file into phpMyAdmin to create all tables and sample data

-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS `creativecollabDB` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `creativecollabDB`;

-- Drop existing tables if they exist (for clean import)
DROP TABLE IF EXISTS `Reminders`;
DROP TABLE IF EXISTS `Payments`;
DROP TABLE IF EXISTS `ProjectDeadlines`;
DROP TABLE IF EXISTS `ProjectUsers`;
DROP TABLE IF EXISTS `Installments`;
DROP TABLE IF EXISTS `Bookmarks`;
DROP TABLE IF EXISTS `DesignerStatuses`;
DROP TABLE IF EXISTS `GamePrices`;
DROP TABLE IF EXISTS `ProductPrices`;
DROP TABLE IF EXISTS `ProductTags`;
DROP TABLE IF EXISTS `Products`;
DROP TABLE IF EXISTS `Projects`;
DROP TABLE IF EXISTS `Games`;
DROP TABLE IF EXISTS `Stores`;
DROP TABLE IF EXISTS `Categories`;
DROP TABLE IF EXISTS `Users`;

-- Create Categories table
CREATE TABLE `Categories` (
    `CategoryId` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`CategoryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Games table
CREATE TABLE `Games` (
    `GameId` int NOT NULL AUTO_INCREMENT,
    `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Genre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Platform` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `ReleaseDate` datetime(6) NULL,
    `Developer` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    PRIMARY KEY (`GameId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Stores table
CREATE TABLE `Stores` (
    `StoreId` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Website` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    PRIMARY KEY (`StoreId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Users table
CREATE TABLE `Users` (
    `UserId` int NOT NULL AUTO_INCREMENT,
    `Username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Products table
CREATE TABLE `Products` (
    `ProductId` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `CategoryId` int NOT NULL,
    `AddedAt` datetime(6) NOT NULL,
    PRIMARY KEY (`ProductId`),
    CONSTRAINT `FK_Products_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`CategoryId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create GamePrices table
CREATE TABLE `GamePrices` (
    `PriceId` int NOT NULL AUTO_INCREMENT,
    `GameId` int NOT NULL,
    `StoreId` int NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    `DateTracked` datetime(6) NOT NULL,
    PRIMARY KEY (`PriceId`),
    CONSTRAINT `FK_GamePrices_Games_GameId` FOREIGN KEY (`GameId`) REFERENCES `Games` (`GameId`) ON DELETE CASCADE,
    CONSTRAINT `FK_GamePrices_Stores_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `Stores` (`StoreId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Bookmarks table
CREATE TABLE `Bookmarks` (
    `BookmarkId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `GameId` int NOT NULL,
    `NotifyBelowPrice` decimal(65,30) NULL,
    `CreatedAt` datetime(6) NOT NULL,
    PRIMARY KEY (`BookmarkId`),
    CONSTRAINT `FK_Bookmarks_Games_GameId` FOREIGN KEY (`GameId`) REFERENCES `Games` (`GameId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Bookmarks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create DesignerStatuses table
CREATE TABLE `DesignerStatuses` (
    `DesignerStatusId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `AvailableFrom` datetime(6) NOT NULL,
    `AvailableTo` datetime(6) NOT NULL,
    `StatusMessage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    PRIMARY KEY (`DesignerStatusId`),
    CONSTRAINT `FK_DesignerStatuses_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Projects table
CREATE TABLE `Projects` (
    `ProjectId` int NOT NULL AUTO_INCREMENT,
    `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `OwnerUserId` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`ProjectId`),
    CONSTRAINT `FK_Projects_Users_OwnerUserId` FOREIGN KEY (`OwnerUserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create ProductPrices table
CREATE TABLE `ProductPrices` (
    `ProductPriceId` int NOT NULL AUTO_INCREMENT,
    `ProductId` int NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    `StoreId` int NOT NULL,
    `DateTracked` datetime(6) NOT NULL,
    PRIMARY KEY (`ProductPriceId`),
    CONSTRAINT `FK_ProductPrices_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProductPrices_Stores_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `Stores` (`StoreId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create ProductTags table
CREATE TABLE `ProductTags` (
    `ProductTagId` int NOT NULL AUTO_INCREMENT,
    `ProductId` int NOT NULL,
    `Tag` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    PRIMARY KEY (`ProductTagId`),
    CONSTRAINT `FK_ProductTags_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Installments table
CREATE TABLE `Installments` (
    `InstallmentId` int NOT NULL AUTO_INCREMENT,
    `ProjectId` int NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `DueDate` datetime(6) NOT NULL,
    `IsPaid` tinyint(1) NOT NULL,
    PRIMARY KEY (`InstallmentId`),
    CONSTRAINT `FK_Installments_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`ProjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Payments table
CREATE TABLE `Payments` (
    `PaymentId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `GameId` int NULL,
    `ProjectId` int NULL,
    `Amount` decimal(65,30) NOT NULL,
    `PaymentMethod` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `PaymentDate` datetime(6) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `Type` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`PaymentId`),
    CONSTRAINT `FK_Payments_Games_GameId` FOREIGN KEY (`GameId`) REFERENCES `Games` (`GameId`),
    CONSTRAINT `FK_Payments_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`ProjectId`),
    CONSTRAINT `FK_Payments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create ProjectDeadlines table
CREATE TABLE `ProjectDeadlines` (
    `DeadlineId` int NOT NULL AUTO_INCREMENT,
    `ProjectId` int NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `DueDate` datetime(6) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `IsCompleted` tinyint(1) NOT NULL,
    PRIMARY KEY (`DeadlineId`),
    CONSTRAINT `FK_ProjectDeadlines_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`ProjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create ProjectUsers table
CREATE TABLE `ProjectUsers` (
    `ProjectUserId` int NOT NULL AUTO_INCREMENT,
    `ProjectId` int NOT NULL,
    `UserId` int NOT NULL,
    `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`ProjectUserId`),
    CONSTRAINT `FK_ProjectUsers_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`ProjectId`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProjectUsers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create Reminders table
CREATE TABLE `Reminders` (
    `ReminderId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `GameId` int NULL,
    `ProjectId` int NULL,
    `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `DueDate` datetime(6) NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    PRIMARY KEY (`ReminderId`),
    CONSTRAINT `FK_Reminders_Games_GameId` FOREIGN KEY (`GameId`) REFERENCES `Games` (`GameId`),
    CONSTRAINT `FK_Reminders_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`ProjectId`),
    CONSTRAINT `FK_Reminders_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create indexes
CREATE INDEX `IX_Bookmarks_GameId` ON `Bookmarks` (`GameId`);
CREATE INDEX `IX_Bookmarks_UserId` ON `Bookmarks` (`UserId`);
CREATE UNIQUE INDEX `IX_DesignerStatuses_UserId` ON `DesignerStatuses` (`UserId`);
CREATE INDEX `IX_GamePrices_GameId` ON `GamePrices` (`GameId`);
CREATE INDEX `IX_GamePrices_StoreId` ON `GamePrices` (`StoreId`);
CREATE INDEX `IX_Installments_ProjectId` ON `Installments` (`ProjectId`);
CREATE INDEX `IX_Payments_GameId` ON `Payments` (`GameId`);
CREATE INDEX `IX_Payments_ProjectId` ON `Payments` (`ProjectId`);
CREATE INDEX `IX_Payments_UserId` ON `Payments` (`UserId`);
CREATE INDEX `IX_ProductPrices_ProductId` ON `ProductPrices` (`ProductId`);
CREATE INDEX `IX_ProductPrices_StoreId` ON `ProductPrices` (`StoreId`);
CREATE INDEX `IX_Products_CategoryId` ON `Products` (`CategoryId`);
CREATE INDEX `IX_ProductTags_ProductId` ON `ProductTags` (`ProductId`);
CREATE INDEX `IX_ProjectDeadlines_ProjectId` ON `ProjectDeadlines` (`ProjectId`);
CREATE INDEX `IX_ProjectUsers_ProjectId` ON `ProjectUsers` (`ProjectId`);
CREATE INDEX `IX_ProjectUsers_UserId` ON `ProjectUsers` (`UserId`);
CREATE INDEX `IX_Reminders_GameId` ON `Reminders` (`GameId`);
CREATE INDEX `IX_Reminders_ProjectId` ON `Reminders` (`ProjectId`);
CREATE INDEX `IX_Reminders_UserId` ON `Reminders` (`UserId`);

-- Insert sample data

-- Insert Categories
INSERT INTO `Categories` (`Name`) VALUES
('Action'),
('Adventure'),
('RPG'),
('Strategy'),
('Sports'),
('Puzzle'),
('Simulation'),
('Horror');

-- Insert Stores
INSERT INTO `Stores` (`Name`, `Website`) VALUES
('Steam', 'https://store.steampowered.com'),
('Epic Games Store', 'https://store.epicgames.com'),
('GOG', 'https://www.gog.com'),
('Humble Bundle', 'https://www.humblebundle.com'),
('Amazon Games', 'https://amazon.com/games'),
('PlayStation Store', 'https://store.playstation.com'),
('Xbox Store', 'https://www.xbox.com/games'),
('Nintendo eShop', 'https://www.nintendo.com/games');

-- Insert Users
INSERT INTO `Users` (`Username`, `Email`, `PasswordHash`, `CreatedAt`, `Role`) VALUES
('john_doe', 'john@example.com', '$2a$10$dummyhash123', '2024-01-01 10:00:00', 'User'),
('jane_smith', 'jane@example.com', '$2a$10$dummyhash456', '2024-01-02 11:00:00', 'User'),
('admin_user', 'admin@creativecollab.com', '$2a$10$dummyhash789', '2024-01-01 09:00:00', 'Admin'),
('game_dev', 'dev@example.com', '$2a$10$dummyhash101', '2024-01-03 12:00:00', 'Developer'),
('designer_pro', 'designer@example.com', '$2a$10$dummyhash202', '2024-01-04 13:00:00', 'Designer');

-- Insert Games
INSERT INTO `Games` (`Title`, `Genre`, `Platform`, `Description`, `ReleaseDate`, `Developer`) VALUES
('The Witcher 3: Wild Hunt', 'RPG', 'PC, PS4, Xbox One, Switch', 'An epic role-playing game with a vast open world', '2015-05-19', 'CD Projekt Red'),
('Cyberpunk 2077', 'RPG', 'PC, PS4, PS5, Xbox One, Xbox Series X', 'An open-world action-adventure story', '2020-12-10', 'CD Projekt Red'),
('Red Dead Redemption 2', 'Action-Adventure', 'PC, PS4, Xbox One', 'A western action-adventure game', '2018-10-26', 'Rockstar Games'),
('Elden Ring', 'Action RPG', 'PC, PS4, PS5, Xbox One, Xbox Series X', 'An action role-playing game', '2022-02-25', 'FromSoftware'),
('God of War Ragnarök', 'Action-Adventure', 'PS4, PS5', 'An action-adventure game', '2022-11-09', 'Santa Monica Studio'),
('Minecraft', 'Sandbox', 'PC, Mobile, Console', 'A 3D sandbox game', '2011-11-18', 'Mojang'),
('Fortnite', 'Battle Royale', 'PC, Mobile, Console', 'A free-to-play battle royale game', '2017-07-25', 'Epic Games'),
('League of Legends', 'MOBA', 'PC', 'A multiplayer online battle arena game', '2009-10-27', 'Riot Games'),
('Valorant', 'Tactical Shooter', 'PC', 'A 5v5 tactical shooter game', '2020-06-02', 'Riot Games'),
('Among Us', 'Social Deduction', 'PC, Mobile', 'A multiplayer social deduction game', '2018-06-15', 'InnerSloth');

-- Insert GamePrices
INSERT INTO `GamePrices` (`GameId`, `StoreId`, `Price`, `DateTracked`) VALUES
(1, 1, 29.99, '2024-01-15 10:00:00'),
(1, 2, 24.99, '2024-01-15 10:00:00'),
(2, 1, 59.99, '2024-01-15 10:00:00'),
(2, 3, 49.99, '2024-01-15 10:00:00'),
(3, 1, 39.99, '2024-01-15 10:00:00'),
(3, 2, 34.99, '2024-01-15 10:00:00'),
(4, 1, 59.99, '2024-01-15 10:00:00'),
(4, 6, 69.99, '2024-01-15 10:00:00'),
(5, 6, 69.99, '2024-01-15 10:00:00'),
(6, 1, 26.95, '2024-01-15 10:00:00'),
(7, 2, 0.00, '2024-01-15 10:00:00'),
(8, 1, 0.00, '2024-01-15 10:00:00'),
(9, 1, 0.00, '2024-01-15 10:00:00'),
(10, 1, 4.99, '2024-01-15 10:00:00');

-- Insert Products
INSERT INTO `Products` (`Name`, `Description`, `CategoryId`, `AddedAt`) VALUES
('Gaming Mouse', 'High-performance gaming mouse with RGB lighting', 1, '2024-01-10 09:00:00'),
('Mechanical Keyboard', 'Mechanical gaming keyboard with customizable switches', 1, '2024-01-10 09:00:00'),
('Gaming Headset', '7.1 surround sound gaming headset', 1, '2024-01-10 09:00:00'),
('Gaming Monitor', '27-inch 144Hz gaming monitor', 1, '2024-01-10 09:00:00'),
('Gaming Chair', 'Ergonomic gaming chair with lumbar support', 1, '2024-01-10 09:00:00');

-- Insert ProductPrices
INSERT INTO `ProductPrices` (`ProductId`, `Price`, `StoreId`, `DateTracked`) VALUES
(1, 79.99, 5, '2024-01-15 10:00:00'),
(1, 89.99, 5, '2024-01-15 10:00:00'),
(2, 149.99, 5, '2024-01-15 10:00:00'),
(2, 159.99, 5, '2024-01-15 10:00:00'),
(3, 99.99, 5, '2024-01-15 10:00:00'),
(4, 299.99, 5, '2024-01-15 10:00:00'),
(5, 199.99, 5, '2024-01-15 10:00:00');

-- Insert ProductTags
INSERT INTO `ProductTags` (`ProductId`, `Tag`, `IsActive`) VALUES
(1, 'RGB', 1),
(1, 'Wireless', 1),
(2, 'Mechanical', 1),
(2, 'RGB', 1),
(3, 'Surround Sound', 1),
(3, 'Noise Cancelling', 1),
(4, '144Hz', 1),
(4, '4K', 1),
(5, 'Ergonomic', 1),
(5, 'Adjustable', 1);

-- Insert Projects
INSERT INTO `Projects` (`Title`, `Description`, `OwnerUserId`, `CreatedAt`, `Status`) VALUES
('Mobile Game Development', 'Creating a new mobile puzzle game', 1, '2024-01-05 14:00:00', 'In Progress'),
('Website Redesign', 'Redesigning company website', 2, '2024-01-06 15:00:00', 'Planning'),
('Game Asset Creation', 'Creating 3D models for RPG game', 4, '2024-01-07 16:00:00', 'In Progress'),
('UI/UX Design', 'Designing user interface for mobile app', 5, '2024-01-08 17:00:00', 'Completed'),
('Marketing Campaign', 'Digital marketing campaign for new product', 3, '2024-01-09 18:00:00', 'Planning');

-- Insert ProjectUsers
INSERT INTO `ProjectUsers` (`ProjectId`, `UserId`, `Role`) VALUES
(1, 1, 'Project Manager'),
(1, 4, 'Developer'),
(1, 5, 'Designer'),
(2, 2, 'Project Manager'),
(2, 5, 'Designer'),
(3, 4, 'Lead Artist'),
(3, 1, 'Project Manager'),
(4, 5, 'Lead Designer'),
(5, 3, 'Marketing Manager'),
(5, 2, 'Content Creator');

-- Insert ProjectDeadlines
INSERT INTO `ProjectDeadlines` (`ProjectId`, `Title`, `DueDate`, `Description`, `IsCompleted`) VALUES
(1, 'Alpha Version Complete', '2024-02-15 17:00:00', 'Complete alpha version with basic gameplay', 0),
(1, 'Beta Testing', '2024-03-01 17:00:00', 'Begin beta testing phase', 0),
(2, 'Design Mockups', '2024-01-25 17:00:00', 'Complete initial design mockups', 0),
(3, 'Character Models', '2024-02-10 17:00:00', 'Complete all character 3D models', 0),
(4, 'Final Design Review', '2024-01-20 17:00:00', 'Final design review and approval', 1);

-- Insert Installments
INSERT INTO `Installments` (`ProjectId`, `Amount`, `DueDate`, `IsPaid`) VALUES
(1, 5000.00, '2024-02-01 00:00:00', 1),
(1, 7500.00, '2024-03-01 00:00:00', 0),
(2, 3000.00, '2024-01-25 00:00:00', 1),
(3, 4000.00, '2024-02-01 00:00:00', 0),
(4, 2500.00, '2024-01-15 00:00:00', 1);

-- Insert Payments
INSERT INTO `Payments` (`UserId`, `GameId`, `ProjectId`, `Amount`, `PaymentMethod`, `PaymentDate`, `Description`, `Type`) VALUES
(1, 1, NULL, 29.99, 'Credit Card', '2024-01-10 14:30:00', 'Purchase of The Witcher 3', 'Game Purchase'),
(2, 2, NULL, 59.99, 'PayPal', '2024-01-11 16:45:00', 'Purchase of Cyberpunk 2077', 'Game Purchase'),
(1, NULL, 1, 5000.00, 'Bank Transfer', '2024-01-12 09:15:00', 'Project milestone payment', 'Project Payment'),
(3, 3, NULL, 39.99, 'Credit Card', '2024-01-13 11:20:00', 'Purchase of Red Dead Redemption 2', 'Game Purchase'),
(2, NULL, 2, 3000.00, 'PayPal', '2024-01-14 13:40:00', 'Website design project payment', 'Project Payment');

-- Insert Bookmarks
INSERT INTO `Bookmarks` (`UserId`, `GameId`, `NotifyBelowPrice`, `CreatedAt`) VALUES
(1, 4, 49.99, '2024-01-12 10:00:00'),
(2, 5, 59.99, '2024-01-13 11:00:00'),
(3, 1, 19.99, '2024-01-14 12:00:00'),
(1, 6, 19.99, '2024-01-15 13:00:00'),
(2, 7, 0.00, '2024-01-16 14:00:00');

-- Insert DesignerStatuses
INSERT INTO `DesignerStatuses` (`UserId`, `AvailableFrom`, `AvailableTo`, `StatusMessage`) VALUES
(5, '2024-01-15 09:00:00', '2024-01-20 17:00:00', 'Available for new design projects'),
(4, '2024-01-16 10:00:00', '2024-01-25 18:00:00', 'Working on game assets, limited availability');

-- Insert Reminders
INSERT INTO `Reminders` (`UserId`, `GameId`, `ProjectId`, `Title`, `DueDate`, `Note`) VALUES
(1, 4, NULL, 'Check Elden Ring price drop', '2024-01-20 10:00:00', 'Wait for sale below $50'),
(2, NULL, 2, 'Website design review meeting', '2024-01-18 14:00:00', 'Prepare design mockups'),
(3, 1, NULL, 'The Witcher 3 DLC release', '2024-01-25 12:00:00', 'New DLC coming out'),
(1, NULL, 1, 'Mobile game alpha deadline', '2024-02-15 17:00:00', 'Complete alpha version'),
(4, NULL, 3, 'Character model submission', '2024-02-10 16:00:00', 'Submit all character models');

-- Success message
SELECT 'Database setup completed successfully! All tables created and populated with sample data.' AS Status;
