-- CREACIÓN DE BASE DE DATOS Y TABLAS --

CREATE DATABASE DbTest -- Creamos la base de datos con el nombre DbTest

USE DbTest -- Seleccionamos la db que creamos para poder trabajar con ella

CREATE TABLE Carrier(
    CarrierId int not null IDENTITY, -- Le decimos que este campo será auto incremental
	Name varchar(20) not null,
	Capacity int not null,
	constraint pk_carrier PRIMARY KEY(CarrierId) -- Le indicamos que CarrierId será nuestra primary key
);

CREATE TABLE Costos(
    CarrierId int not null,
	Zona varchar(20) not null,
	Costo int not null,
	Tiempo_Entrega int not null,
	constraint fk_carrier foreign key(CarrierId) references Carrier(CarrierId) -- Le indicamos que CarrierId
	-- será un campo de tipo clave foránea, que hace referencia a la tabla Carrier y al campo CarrierId
)

-- Ceación de la ultima tabla
CREATE TABLE Cantidad_Envios(
    Zona varchar(20) not null,
	Mes int not null,
	Cantidad_Envios int not null
);


/* AHORA EMPEZAMOS A INSERTAR DATOS EN NUESTRAS TABLAS */

-- Insertamos en la tabla Carrier
insert into Carrier values('Carrier A', 10000)
insert into Carrier values('Carrier B', 10000)
insert into Carrier values('Carrier C', 10000)

-- Insertamos en la tabla Costos
insert into Costos values(1, 'AMBA', 10, 3)
insert into Costos values(1, 'Bs.As', 20, 5)
insert into Costos values(1, 'Resto', 50, 7)

insert into Costos values(2, 'AMBA', 15, 2)
insert into Costos values(2, 'Bs.As', 19, 4)
insert into Costos values(1, 'Resto', 55, 6)

insert into Costos values(3, 'AMBA', 20, 1)

-- Insertamos en la tabla Cantidad_Envios
insert into Cantidad_Envios values('AMBA', 1, 40000)
insert into Cantidad_Envios values('Bs.As', 1, 50000)
insert into Cantidad_Envios values('Resto', 1, 60000)

-- NOTA: Los insert tambien podrian hacerse con subquerys.

-- RESPUESTAS A LAS PREGUNTAS --

/* 1.- Obtener para el mes 1 cuánto costaría enviar con cada carrier los envíos de
la tabla Cantidad de envíos. */

-- Creamos un procedimiento almacenado el cual se va a ocuptar de devolverme el costo de cada carrier.
-- En este caso, me pide como parametro la zona del envio y la id del carrier para calcular el costo.

CREATE PROCEDURE ObtenerCostos @zona varchar(20), @id_carrier int
AS
    select top 1 Carrier.Name, Costos.Zona, Costos.Costo, Costos.Costo * 
    (select Cantidad_Envios from Cantidad_Envios where Zona = @zona) as 'Total',
    (select Cantidad_Envios from Cantidad_Envios where Zona = @zona) as 'Cantidad De Envios' from Carrier
    inner join Costos on Costos.CarrierId = @id_carrier
GO

-- Ejemplo de como ejecutar nuestro procedimiendo almacenado
exec ObtenerCostos @zona = 'AMBA', @id_carrier = 1

/*
2.- ¿Que propuesta harías para el mes 1 considerando un presupuesto de
$3.000.000?

-- Mi propuesta sería agregar un nuevo Carrier, por ejemplo Carrier A - prop con la misma capacidad.
*/
insert into Carrier values('Carrier A - prop', 10000)