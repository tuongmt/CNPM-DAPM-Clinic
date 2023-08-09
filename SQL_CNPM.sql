USE MASTER
IF EXISTS (SELECT * FROM SYSDATABASES WHERE NAME = 'DoAnCNPM')
DROP DATABASE DoAnCNPM
GO
CREATE DATABASE DoAnCNPM
GO
USE DoAnCNPM
GO


--TABLE
CREATE TABLE AdminUser
(
  IdAdminUser INT IDENTITY(1,1) NOT NULL,
  Username CHAR(100) NOT NULL,
  NameAdminUser NVARCHAR(100) NOT NULL,
  Role NVARCHAR(100) NOT NULL,
  Password CHAR(100) NOT NULL,
  PRIMARY KEY CLUSTERED (IdAdminUser ASC)
);

CREATE TABLE Category
(
  IdCat INT IDENTITY(1,1) NOT NULL,
  NameCat NVARCHAR(100) NOT NULL,
  PRIMARY KEY CLUSTERED (IdCat ASC)
);

CREATE TABLE Doctor
(
  IdDoctor INT IDENTITY(1,1) NOT NULL,
  NameDoctor NVARCHAR(100) NOT NULL,
  PhoneDoctor CHAR(11) NOT NULL,
  PRIMARY KEY CLUSTERED (IdDoctor ASC)
);

CREATE TABLE PriceList(
	IdPriceList INT IDENTITY(1,1) NOT NULL,
	NamePriceList NVARCHAR(100) NOT NULL,
	Price FLOAT(53) NOT NULL,
	IdCat INT NOT NULL,
	PRIMARY KEY CLUSTERED (IdPriceList ASC),
	FOREIGN KEY (IdCat) REFERENCES Category(IdCat)
);

CREATE TABLE Staff
(
  IdStaff INT IDENTITY(1,1) NOT NULL,
  NameStaff NVARCHAR(100) NOT NULL,
  PhoneStaff CHAR(11) NOT NULL,
  MailStaff VARCHAR(100) NOT NULL,
  PRIMARY KEY CLUSTERED (IdStaff ASC),
  UNIQUE (PhoneStaff)
);

CREATE TABLE Customer
(
  IdCus INT IDENTITY(1,1) NOT NULL,
  NameCus NVARCHAR(100) NOT NULL,
  PhoneCus CHAR(11) NOT NULL,
  PRIMARY KEY CLUSTERED (IdCus ASC),
);

CREATE TABLE Form
(
  IdForm INT IDENTITY(1,1) NOT NULL,
  ExamTime DATETIME,
  IdDoctor INT NOT NULL,
  IdCus INT NOT NULL,
  IdStaff INT NOT NULL
  PRIMARY KEY CLUSTERED (IdForm ASC),
  FOREIGN KEY (IdDoctor) REFERENCES Doctor(IdDoctor),
  FOREIGN KEY (IdCus) REFERENCES Customer(IdCus),
  FOREIGN KEY (IdStaff) REFERENCES Staff(IdStaff)
);

CREATE TABLE DetailForm
(
  IdDF INT IDENTITY(1,1) NOT NULL,
  Quantity INT NOT NULL,
  TotalMoney INT,
  IsExamined BIT,
  IsPaid BIT,
  IdForm INT NOT NULL,
  IdPriceList INT NOT NULL,
  PRIMARY KEY CLUSTERED (IdDF ASC),
  FOREIGN KEY (IdForm) REFERENCES Form(IdForm),
  FOREIGN KEY (IdPriceList) REFERENCES PriceList(IdPriceList)
);

CREATE TABLE DiseaseStatisticList
(
  IdDSL INT IDENTITY(1,1) NOT NULL,
  Dianose NVARCHAR(100) NOT NULL,
  IdForm INT NOT NULL,
  PRIMARY KEY CLUSTERED (IdDSL ASC),
  FOREIGN KEY (IdForm) REFERENCES Form(IdForm)
);

--TRIGGER
CREATE TRIGGER SetIsPayToDefault
ON DetailForm
AFTER INSERT
AS
BEGIN
  UPDATE DetailForm
  SET IsPaid = 0
  FROM inserted
  WHERE DetailForm.IdDF = inserted.IdDF;
END;

CREATE TRIGGER SetIsExaminedToDefault
ON DetailForm
AFTER INSERT
AS
BEGIN
  UPDATE DetailForm
  SET IsExamined = 0
  FROM inserted
  WHERE DetailForm.IdDF = inserted.IdDF;
END;

CREATE TRIGGER UpdateTotalMoney
ON DetailForm
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE df
    SET TotalMoney = df.Quantity * pl.Price
    FROM DetailForm df
    JOIN PriceList pl ON df.IdPriceList = pl.IdPriceList
    JOIN inserted i ON df.IdDF = i.IdDF
END;


--DATA
INSERT INTO AdminUser(Username,NameAdminUser, Role, Password) VALUES
('tuong',N'Tường',N'Nhân viên','123456');

INSERT INTO Category (NameCat) VALUES 
(N'Kiểm tra tổng quát - General checkup'), 
(N'Tim mạch - Cardiology'), 
(N'Da liễu - Dermatology'), 
(N'Thần kinh - Neurology'), 
(N'Nhi khoa - Pediatrics'),
(N'Răng Hàm Mặt - Dentistry'), 
(N'Khám mặt - Ophthalmology'), 
(N'Hô hấp - Respiratory'), 
(N'Tai mũi họng - Otorhinolaryngology'), 
(N'Chẩn đoán hình ảnh - Radiology'),
(N'Nội tiết - Endocrinology'), 
(N'Ung thư - Oncology'), 
(N'Thú y - Veterinary'), 
(N'Tiêu hóa - Gastroenterology'), 
(N'Tâm lý - Psychology');

INSERT INTO Doctor (NameDoctor, PhoneDoctor) VALUES
(N'Mã Tuấn Tường','0987123456'),
(N'Lê Trần Quang Tín','0987123123'),
(N'Lê Lâm Chí Dĩnh','0987333456'),
(N'Nguyễn Văn An', '0987111222'),
(N'Phạm Thị Ly', '0987333444'),
(N'Trần Xuân Cường', '0987555666'),
(N'Lê Thị Giang', '0987999881'),
(N'Nguyễn Văn Huy', '0987111992'),
(N'Phạm Thị Ý', '0987222443');

INSERT INTO PriceList (NamePriceList, Price, IdCat) VALUES 
(N'Kiểm tra sức khỏe cơ bản - Basic checkup', 1000000, 1),
(N'Kiểm tra sức khỏe nâng cao  - Advanced checkup',1500000,1),
(N'Kiểm tra tim - Heart check',3000000,2),
(N'Tư vấn da - Skin consultation', 1200000, 3), 
(N'Đánh giá thần kinh - Neurological evaluation', 5000000, 4), 
(N'Khám sức khỏe trẻ em - Child wellness exam', 900000, 5),
(N'Nhổ răng - Tooth extraction', 200000, 6),
(N'Thử thị lực - Vision test', 500000, 7),
(N'Xét nghiệm phế quản - Bronchoscopy', 3500000, 8),
(N'Khám tai mũi họng - ENT examination', 800000, 9), 
(N'CT scanner', 4500000, 10),
(N'Khám sức khỏe răng miệng  - Dental checkup', 250000, 11),
(N'Xét nghiệm ung thư - Cancer screening', 4000000, 12),
(N'Khám thú y cơ bản - Basic veterinary checkup', 150000, 13),
(N'Khám tiêu hóa - Gastrointestinal examination', 1000000, 14), 
(N'Tư vấn tâm lý - Psychological counseling', 800000, 15);

INSERT INTO Staff (NameStaff, PhoneStaff, MailStaff) VALUES
(N'Phạm Ngọc Anh','0123789456','giang@gmail.com'),
(N'Nguyễn Kiều Trang','0123456456','trang@gmail.com'),
(N'Lê Thị Nhung','0123123456','nhung@gmail.com'),
(N'Nguyễn Văn Bình', '0987999888', 'nguyenb@gmail.com'),
(N'Vũ Thị Linh', '0987666777', 'vutc@gmail.com'),
(N'Lê Xuân Duy', '0987444222', 'lexuand@gmail.com'),
(N'Nguyễn Thị Quỳnh', '0987777888', 'nguyenj@gmail.com'),
(N'Vũ Văn Kiên', '0987555333', 'vuvank@gmail.com'),
(N'Lê Thị Thu', '0987444777', 'lethil@gmail.com');

INSERT INTO Customer (NameCus, PhoneCus) VALUES
(N'Nguyễn Thị Mỹ Diệu','0945612378'),
(N'Nguyễn Hồng Hải','0789698478'),
(N'Trần Thị Kim Oanh','0123142362'),
(N'Ngô Văn Sang', '0987222333'),
(N'Lệ Thị Hoa', '0987333111'),
(N'Hoàng Văn Thụ', '0987555999'),
(N'Trần Văn Minh', '0987222999'),
(N'Lê Thị Nhi', '0987333112'),
(N'Hoàng Văn Uy', '0987555111');


INSERT INTO Form (ExamTime,  IdDoctor, IdCus,  IdStaff) VALUES
('2023-07-23 08:00:00', 1, 1, 1), 
('2023-08-15 08:00:00', 2, 2, 2),
('2023-08-18 08:00:00', 3, 3, 3),
('2023-09-01 08:00:00', 1, 4, 4), 
('2023-09-02 08:00:00', 2, 5, 5),
('2023-09-03 08:00:00', 3, 6, 6),
('2023-09-04 08:00:00', 1, 7, 7), 
('2023-09-05 08:00:00', 2, 8, 8),
('2023-09-06 08:00:00', 3, 9, 9);

INSERT INTO DetailForm (Quantity, IdForm, IdPriceList, IsExamined, IsPaid) VALUES
(1, 1, 1, 0 , 0), 
(1, 2, 4, 0 , 0),
(2, 3, 5, 0 , 0),
(3, 4, 6, 0 , 0), 
(2, 5, 7, 0 , 0),
(1, 6, 10, 0 , 0),
(2, 7, 11, 0 , 0), 
(1, 8, 12, 0 , 0),
(3, 9, 15, 0 , 0);

INSERT INTO DiseaseStatisticList (Dianose, IdForm) VALUES
(N'Huyết áp cao - High blood pressure', 1),
(N'Viêm da cơ địa - Eczema', 2), 
(N'Đau nửa đầu - Migraine', 3),
(N'Nhổ răng mọc lệch - Crooked tooth extraction', 4),
(N'Mắt cận thị - Myopia', 5), 
(N'Hắc lào - Tuberculosis', 6),
(N'Rối loạn nội tiết - Endocrine disorder', 7),
(N'Ung thư vú - Breast cancer', 8), 
(N'Bệnh viêm đường hô hấp - Respiratory infection', 9);



SELECT * FROM AdminUser
SELECT * FROM Category
SELECT * FROM Customer
SELECT * FROM DiseaseStatisticList
SELECT * FROM Doctor
SELECT * FROM Form
SELECT * FROM DetailForm
SELECT * FROM PriceList
SELECT * FROM Staff

