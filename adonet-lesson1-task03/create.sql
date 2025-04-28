CREATE DATABASE Library COLLATE Cyrillic_General_CI_AS;
go

use Library;

  CREATE TABLE Authors (
      Id INT IDENTITY(1,1) PRIMARY KEY,
      FirstName NVARCHAR(50) NOT NULL,
      LastName NVARCHAR(50) NOT NULL
  );
  go
