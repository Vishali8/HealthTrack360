CREATE DATABASE HealthTracker360;
USE HealthTracker360;

CREATE TABLE Patients (
	PatientID INT PRIMARY KEY IDENTITY,
	FullName NVARCHAR(100),
	Gender CHAR(1),
	DOB DATE,
	ContactNumber NVARCHAR(15),
	Email NVARCHAR(100),
	CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Visits(
	VisitID INT PRIMARY KEY IDENTITY,
	PatientID INT FOREIGN KEY REFERENCES Patients(PatientID),
	VisitDate DATETIME,
	Department NVARCHAR(50),
	DoctorName NVARCHAR(100),
	Reason NVARCHAR(255)
);
CREATE TABLE Vitals(
	VitalID INT PRIMARY KEY IDENTITY,
	VisitID INT FOREIGN KEY REFERENCES Visits(VisitID),
	BloodPressure NVARCHAR(10),
	SugarLevel DECIMAL(5,2),
	PulseRate INT,
	Temperature DECIMAL(4,1),
	RecordedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Alerts(
	AlertTD INT PRIMARY KEY IDENTITY,
	VitalID INT FOREIGN KEY REFERENCES Vitals(VitalID),
	AlertType NVARCHAR(50),
	Description NVARCHAR(255),
	TriggeredAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE AuditLog(
	LogID INT PRIMARY KEY IDENTITY,
	TableName NVARCHAR(50),
	ActionType NVARCHAR(10),
	RecordID INT,
	OldValue NVARCHAR(MAX),
	NewValue NVARCHAR(MAX),
	ChangedBy NVARCHAR(100),
	ChangedAt DATETIME DEFAULT GETDATE()
);

CREATE TRIGGER trg_AbnormalBP
ON Vitals
AFTER INSERT
AS
BEGIN
	INSERT INTO Alerts(VitalID, AlertType, Description)
	SELECT VitalID, 'High BP', 'Blood Pressure exceeds safe range'
	FROM inserted
	WHERE CAST(SUBSTRING(BloodPressure, 1, CHARINDEX('/',BloodPressure)-1) AS INT)>140;
END;

SELECT * FROM Patients;

INSERT INTO Patients (FullName, Gender, DOB, ContactNumber, Email)
VALUES 
('Arun Kumar', 'M', '1990-05-12', '9876543210', 'arun.kumar@example.com'),
('Meena Rani', 'F', '1985-11-23', '9123456789', 'meena.rani@example.com'),
('Rahul Sharma', 'M', '1992-07-08', '9988776655', 'rahul.sharma@example.com'),
('Priya Das', 'F', '1995-03-15', '9090909090', 'priya.das@example.com'),
('Karthik Raj', 'M', '1988-09-30', '9012345678', 'karthik.raj@example.com'),
('Anjali Nair', 'F', '1993-01-20', '9345678901', 'anjali.nair@example.com'),
('Suresh Babu', 'M', '1979-06-25', '9445566778', 'suresh.babu@example.com'),
('Divya Menon', 'F', '1996-12-05', '9556677889', 'divya.menon@example.com'),
('Vikram Singh', 'M', '1983-04-17', '9667788990', 'vikram.singh@example.com'),
('Neha Verma', 'F', '1991-08-11', '9778899001', 'neha.verma@example.com');

CREATE TABLE Recruiter(
	RecruiterId INT PRIMARY KEY IDENTITY,
	Email NVARCHAR(100) NOT NULL,
	Password NVARCHAR(100) NOT NULL
	);
TRUNCATE TABLe Recruiter;

INSERT INTO Recruiter(Email, Password)
VALUES ('Vishali@gmail.com' , 'Kadhir*8');