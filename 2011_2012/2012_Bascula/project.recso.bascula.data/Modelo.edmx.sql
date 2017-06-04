
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/05/2013 19:22:44
-- Generated from EDMX file: C:\Users\develop\Documents\Visual Studio 2010\Projects\2012_Recso_Bascula_villanubla\project.recso.bascula.data\Modelo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [recso2011DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ConfiguracionApps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConfiguracionApps];
GO
IF OBJECT_ID(N'[dbo].[EmpresaEnObras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpresaEnObras];
GO
IF OBJECT_ID(N'[dbo].[Empresas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Empresas];
GO
IF OBJECT_ID(N'[dbo].[FormasPago]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FormasPago];
GO
IF OBJECT_ID(N'[dbo].[HistoricoAlbaranes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HistoricoAlbaranes];
GO
IF OBJECT_ID(N'[dbo].[logFormasPago]', 'U') IS NOT NULL
    DROP TABLE [dbo].[logFormasPago];
GO
IF OBJECT_ID(N'[dbo].[Obras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Obras];
GO
IF OBJECT_ID(N'[dbo].[Residuos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Residuos];
GO
IF OBJECT_ID(N'[dbo].[TipoVehiculoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TipoVehiculoes];
GO
IF OBJECT_ID(N'[dbo].[TransitosActuales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransitosActuales];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Vehiculos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehiculos];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TipoVehiculoes'
CREATE TABLE [dbo].[TipoVehiculoes] (
    [recnum] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(100)  NULL,
    [capacidad] float  NULL
);
GO

-- Creating table 'Vehiculos'
CREATE TABLE [dbo].[Vehiculos] (
    [recnum] bigint IDENTITY(1,1) NOT NULL,
    [recnumEmpresa] bigint  NULL,
    [matriculaVehiculo] varchar(50)  NULL,
    [fechaAlta] varchar(50)  NULL,
    [fechaBaja] varchar(50)  NULL
);
GO

-- Creating table 'EmpresaEnObras'
CREATE TABLE [dbo].[EmpresaEnObras] (
    [recnumEmpresa] bigint  NOT NULL,
    [recnumObra] bigint  NOT NULL,
    [fechaInicio] varchar(20)  NOT NULL,
    [fechaFin] varchar(20)  NOT NULL
);
GO

-- Creating table 'TransitosActuales'
CREATE TABLE [dbo].[TransitosActuales] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [numAlbaran] varchar(50)  NULL,
    [matricula] varchar(50)  NULL,
    [fechaEntrada] varchar(100)  NULL,
    [fechaSalida] varchar(100)  NULL,
    [bruto] varchar(50)  NULL,
    [tara] varchar(50)  NULL,
    [neto] varchar(50)  NULL,
    [importeIVA] varchar(50)  NULL,
    [importeFinal] varchar(50)  NULL,
    [importeSinIva] varchar(50)  NULL,
    [residuo] int  NULL,
    [transportista] bigint  NULL,
    [poseedor] bigint  NULL,
    [productor] bigint  NULL,
    [plantaTransferencia] bigint  NULL,
    [plantaValorizacion] bigint  NULL,
    [obra] bigint  NULL,
    [IVAaplicado] varchar(50)  NULL,
    [tipoVehiculo] varchar(100)  NULL
);
GO

-- Creating table 'ConfiguracionApps'
CREATE TABLE [dbo].[ConfiguracionApps] (
    [ultimoAlbaran] bigint  NOT NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'HistoricoAlbaranes'
CREATE TABLE [dbo].[HistoricoAlbaranes] (
    [id] varchar(50)  NOT NULL,
    [numAlbaran] varchar(50)  NULL,
    [fechaEntrada] varchar(50)  NULL,
    [fechaSalida] varchar(50)  NULL,
    [bruto] varchar(7)  NULL,
    [tara] varchar(7)  NULL,
    [neto] varchar(7)  NULL,
    [matricula] varchar(25)  NULL,
    [precioResiduo] varchar(10)  NULL,
    [ivaAplicado] varchar(10)  NULL,
    [importeSinIVA] varchar(10)  NULL,
    [importeIVA] varchar(10)  NULL,
    [residuo] varchar(50)  NULL,
    [importeFinal] varchar(10)  NULL,
    [NombreTransportista] varchar(100)  NULL,
    [Telefonos] varchar(100)  NULL,
    [TipoVehiculo] varchar(100)  NULL,
    [EmpPagadorNombre] varchar(100)  NULL,
    [EmpPagadorDireccion] varchar(100)  NULL,
    [EntidadGeneradora] varchar(100)  NULL,
    [FormaPago] varchar(100)  NULL,
    [obra] varchar(100)  NULL,
    [LugarGeneracion] varchar(100)  NULL,
    [tipoResiduo] varchar(20)  NULL,
    [empPoseedor] varchar(100)  NULL,
    [metrosCubicos] varchar(20)  NULL,
    [PesoMCubicos] varchar(50)  NULL,
    [empProductor] varchar(100)  NULL,
    [empClienteMilena] varchar(10)  NULL,
    [milenaObra] varchar(10)  NULL,
    [milenaProducto] varchar(10)  NULL
);
GO

-- Creating table 'Residuos'
CREATE TABLE [dbo].[Residuos] (
    [recnum] int IDENTITY(1,1) NOT NULL,
    [nombre] nchar(50)  NULL,
    [descripcion] nchar(100)  NULL,
    [precio] float  NULL,
    [ivaAplicado] float  NULL,
    [tipoMaterial] nchar(8)  NULL,
    [codigoLER] nchar(20)  NULL,
    [milena] varchar(10)  NULL
);
GO

-- Creating table 'FormasPagoes'
CREATE TABLE [dbo].[FormasPagoes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [recnumEmpresa] bigint  NULL,
    [limiteSaldo] float  NULL,
    [saldoActual] float  NULL,
    [ultimaModificacion] varchar(50)  NULL
);
GO

-- Creating table 'Empresas'
CREATE TABLE [dbo].[Empresas] (
    [recnum] bigint IDENTITY(1,1) NOT NULL,
    [nombre] varchar(100)  NULL,
    [razonSocial] varchar(100)  NULL,
    [cif] varchar(50)  NULL,
    [direccion] varchar(500)  NULL,
    [localidad] varchar(50)  NULL,
    [provincia] varchar(50)  NULL,
    [telefono] varchar(50)  NULL,
    [email] varchar(max)  NULL,
    [cuentaBancaria] varchar(100)  NULL,
    [tipoDePago] varchar(25)  NULL,
    [codigoMilena] bigint  NULL,
    [tipoDeEmpresa] varchar(50)  NULL,
    [esmoroso] bit  NULL
);
GO

-- Creating table 'logFormasPagoes'
CREATE TABLE [dbo].[logFormasPagoes] (
    [id] varchar(50)  NOT NULL,
    [recnumEmpresa] bigint  NULL,
    [saldo] float  NULL,
    [fechaModificacion] datetime  NULL,
    [anteriorSaldo] float  NULL,
    [importeAlbaran] float  NULL,
    [tipoModificacion] varchar(50)  NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [loginName] nchar(20)  NOT NULL,
    [password] nchar(20)  NOT NULL,
    [idEmpleado] int  NULL,
    [permiso] varchar(50)  NULL,
    [email] varchar(1024)  NULL,
    [NombreReal] varchar(1024)  NULL
);
GO

-- Creating table 'Obras'
CREATE TABLE [dbo].[Obras] (
    [recnum] bigint IDENTITY(1,1) NOT NULL,
    [denominacion] varchar(max)  NULL,
    [localidad] varchar(50)  NULL,
    [provincia] varchar(50)  NULL,
    [codigoMilena] int  NULL,
    [licenciaMunicipal] varchar(300)  NULL,
    [finicioObra] varchar(50)  NULL,
    [ffinObra] varchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [recnum] in table 'TipoVehiculoes'
ALTER TABLE [dbo].[TipoVehiculoes]
ADD CONSTRAINT [PK_TipoVehiculoes]
    PRIMARY KEY CLUSTERED ([recnum] ASC);
GO

-- Creating primary key on [recnum] in table 'Vehiculos'
ALTER TABLE [dbo].[Vehiculos]
ADD CONSTRAINT [PK_Vehiculos]
    PRIMARY KEY CLUSTERED ([recnum] ASC);
GO

-- Creating primary key on [recnumEmpresa], [recnumObra], [fechaInicio], [fechaFin] in table 'EmpresaEnObras'
ALTER TABLE [dbo].[EmpresaEnObras]
ADD CONSTRAINT [PK_EmpresaEnObras]
    PRIMARY KEY CLUSTERED ([recnumEmpresa], [recnumObra], [fechaInicio], [fechaFin] ASC);
GO

-- Creating primary key on [id] in table 'TransitosActuales'
ALTER TABLE [dbo].[TransitosActuales]
ADD CONSTRAINT [PK_TransitosActuales]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'ConfiguracionApps'
ALTER TABLE [dbo].[ConfiguracionApps]
ADD CONSTRAINT [PK_ConfiguracionApps]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'HistoricoAlbaranes'
ALTER TABLE [dbo].[HistoricoAlbaranes]
ADD CONSTRAINT [PK_HistoricoAlbaranes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [recnum] in table 'Residuos'
ALTER TABLE [dbo].[Residuos]
ADD CONSTRAINT [PK_Residuos]
    PRIMARY KEY CLUSTERED ([recnum] ASC);
GO

-- Creating primary key on [id] in table 'FormasPagoes'
ALTER TABLE [dbo].[FormasPagoes]
ADD CONSTRAINT [PK_FormasPagoes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [recnum] in table 'Empresas'
ALTER TABLE [dbo].[Empresas]
ADD CONSTRAINT [PK_Empresas]
    PRIMARY KEY CLUSTERED ([recnum] ASC);
GO

-- Creating primary key on [id] in table 'logFormasPagoes'
ALTER TABLE [dbo].[logFormasPagoes]
ADD CONSTRAINT [PK_logFormasPagoes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [loginName] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([loginName] ASC);
GO

-- Creating primary key on [recnum] in table 'Obras'
ALTER TABLE [dbo].[Obras]
ADD CONSTRAINT [PK_Obras]
    PRIMARY KEY CLUSTERED ([recnum] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------