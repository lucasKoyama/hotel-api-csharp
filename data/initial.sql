CREATE DATABASE TrybeHotel;
GO

USE TrybeHotel;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Cities] (
    [CityId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [State] nvarchar(max) NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([CityId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    [UserType] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Hotels] (
    [HotelId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [CityId] int NOT NULL,
    CONSTRAINT [PK_Hotels] PRIMARY KEY ([HotelId]),
    CONSTRAINT [FK_Hotels_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([CityId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Rooms] (
    [RoomId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Capacity] int NOT NULL,
    [Image] nvarchar(max) NULL,
    [HotelId] int NOT NULL,
    CONSTRAINT [PK_Rooms] PRIMARY KEY ([RoomId]),
    CONSTRAINT [FK_Rooms_Hotels_HotelId] FOREIGN KEY ([HotelId]) REFERENCES [Hotels] ([HotelId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Bookings] (
    [BookingId] int NOT NULL IDENTITY,
    [CheckIn] datetime2 NOT NULL,
    [CheckOut] datetime2 NOT NULL,
    [GuestQuant] int NOT NULL,
    [UserId] int NOT NULL,
    [RoomId] int NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_Bookings_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Rooms] ([RoomId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Bookings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Bookings_RoomId] ON [Bookings] ([RoomId]);
GO

CREATE INDEX [IX_Bookings_UserId] ON [Bookings] ([UserId]);
GO

CREATE INDEX [IX_Hotels_CityId] ON [Hotels] ([CityId]);
GO

CREATE INDEX [IX_Rooms_HotelId] ON [Rooms] ([HotelId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240126132939_initial', N'7.0.4');
GO

COMMIT;
GO

