-- �������� ������� "�������������"
CREATE TABLE ������������� (
    ID_������������� INT PRIMARY KEY IDENTITY(1,1),
    ��� NVARCHAR(255) NOT NULL
);

-- �������� ������� "������"
CREATE TABLE ������ (
    ID_������ INT PRIMARY KEY IDENTITY(1,1),
    ��������_������ NVARCHAR(255) NOT NULL
);

-- �������� ������� "�����"
CREATE TABLE ����� (
    ID_����� INT PRIMARY KEY IDENTITY(1,1),
    ��������_����� NVARCHAR(255) NOT NULL,
    �������� TEXT,
    �������������_ID INT,
    FOREIGN KEY (�������������_ID) REFERENCES �������������(ID_�������������)
);

-- �������� ������� "��������"
CREATE TABLE �������� (
    ID_�������� INT PRIMARY KEY IDENTITY(1,1),
    ��� NVARCHAR(255) NOT NULL,
    ����_�������� DATE NOT NULL,
    ������_ID INT,
    ���� INT,
    FOREIGN KEY (������_ID) REFERENCES ������(ID_������)
);

-- �������� ������� "������"
CREATE TABLE ������ (
    ID_������ INT PRIMARY KEY IDENTITY(1,1),
    �������_ID INT,
    ����_ID INT,
    ������ CHAR(1),
    ���� DATE,
    FOREIGN KEY (�������_ID) REFERENCES ��������(ID_��������),
    FOREIGN KEY (����_ID) REFERENCES �����(ID_�����)
);

-- �������� ������� "��������"
CREATE TABLE �������� (
    ID_�������� INT PRIMARY KEY IDENTITY(1,1),
    ����_ID INT,
    ����_�������� DATE,
    FOREIGN KEY (����_ID) REFERENCES �����(ID_�����)
);

-- �������� ������� "����������"
CREATE TABLE ���������� (
    ID_���������� INT PRIMARY KEY IDENTITY(1,1),
    ����_ID INT,
    ����� TIME,
    ����� NVARCHAR(255),
    FOREIGN KEY (����_ID) REFERENCES �����(ID_�����)
);

-- �������� ������� "������������"
CREATE TABLE ������������ (
    ID_������������ INT PRIMARY KEY IDENTITY(1,1),
    ����� NVARCHAR(255) NOT NULL,
    ������ NVARCHAR(255) NOT NULL,
    ���� NVARCHAR(50) NOT NULL
);
