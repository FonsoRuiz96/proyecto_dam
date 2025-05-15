-- Tabla: habilidades
CREATE TABLE habilidades (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    cantidad INT DEFAULT 0,
    fisica BOOLEAN DEFAULT true,
    dano BOOLEAN DEFAULT true,
    vida BOOLEAN DEFAULT true,
    objetivo ENUM('mono', 'area', 'auto') NOT NULL,
    coste INT DEFAULT 0,
    descripcion TEXT NOT NULL
);

-- Tabla: personajes_principales
CREATE TABLE personajes_principales (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    hab_basica_id INT NOT NULL,
    hab_secundaria_id INT,
    hab_especial_id INT,
    usar_objeto_id INT,
    graficos VARCHAR(255) NOT NULL,
    vida_max INT NOT NULL CHECK (vida_max >= 1),
    vida_actual INT NOT NULL CHECK (vida_actual >= 0 AND vida_actual <= vida_max),
    energia_max INT NOT NULL CHECK (energia_max >= 1),
    energia_actual INT NOT NULL CHECK (energia_actual >= 0 AND energia_actual <= energia_max),
    fuerza_base INT NOT NULL CHECK (fuerza_base >= 1),
    fuerza_actual INT NOT NULL CHECK (fuerza_actual >= 0 AND fuerza_actual <= fuerza_base),
    RF_base INT NOT NULL CHECK (RF_base >= 1), 
    RF_actual INT NOT NULL CHECK (RF_actual >= 0 AND RF_actual <= RF_base),
    PM_base INT NOT NULL CHECK (PM_base >= 1), 
    PM_actual INT NOT NULL CHECK (PM_actual >= 0 AND PM_actual <= PM_base),
    RM_base INT NOT NULL CHECK (RM_base >= 1),
    RM_actual INT NOT NULL CHECK (RM_actual >= 0 AND RM_actual <= RM_base),
    agilidad INT NOT NULL CHECK (agilidad >= 1),
    aggro INT DEFAULT 1 CHECK (aggro = 1),
    incapacitado BOOLEAN DEFAULT false,
    enfermo BOOLEAN DEFAULT false,
    descripcion TEXT NOT NULL,
    FOREIGN KEY (hab_basica_id) REFERENCES habilidades(id),
    FOREIGN KEY (hab_secundaria_id) REFERENCES habilidades(id),
    FOREIGN KEY (hab_especial_id) REFERENCES habilidades(id),
    FOREIGN KEY (usar_objeto_id) REFERENCES habilidades(id)
);

-- Tabla: npcs
CREATE TABLE npcs (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    dialogo TEXT NOT NULL,
    graficos VARCHAR(255) NOT NULL,
    descripcion TEXT NOT NULL
);

-- Tabla: objetos
CREATE TABLE objetos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    precio INT NOT NULL CHECK (precio >= 10),
    descripcion TEXT NOT NULL
);

-- Tabla: equipos
CREATE TABLE equipos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    personaje_id INT,
    comprado BOOLEAN DEFAULT false, -- He visto, en principio, necesario este campo para indicar si se muestra o no en el "inventario"
    valor_FF INT NOT NULL CHECK ((valor_RF = 0 AND valor_PM = 0 AND valor_RM = 0 AND valor_FF >= 1) OR valor_FF >= 0), 
    valor_RF INT NOT NULL CHECK ((valor_FF = 0 AND valor_PM = 0 AND valor_RM = 0 AND valor_RF >= 1) OR valor_RF >= 0),
    valor_PM INT NOT NULL CHECK ((valor_RF = 0 AND valor_FF = 0 AND valor_RM = 0 AND valor_PM >= 1) OR valor_PM >= 0),
    valor_RM INT NOT NULL CHECK ((valor_RF = 0 AND valor_PM = 0 AND valor_FF = 0 AND valor_RM >= 1) OR valor_RM >= 0), 
    descripcion TEXT NOT NULL,
    FOREIGN KEY (personaje_id) REFERENCES personajes_principales(id)
);

-- Tabla: consumibles
CREATE TABLE consumibles (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    cantidad INT CHECK (cantidad >= 0),
    cantidad_recuperada INT CHECK (cantidad_recuperada >= 10),
    usable_en_combate BOOLEAN DEFAULT true,
    descripcion TEXT NOT NULL
);

-- Tabla: tienen_consumibles (relación personaje-consumible-cantidad)

-- Si el campo "cantidad" está en la tabla "consumibles", esta tabla es innecesaria y/o redundante
CREATE TABLE tienen_consumibles (
    id INT PRIMARY KEY AUTO_INCREMENT,
    personaje_id INT NOT NULL,
    consumible_id INT NOT NULL,
    cantidad INT CHECK (cantidad >= 0),
    FOREIGN KEY (personaje_id) REFERENCES personajes_principales(id),
    FOREIGN KEY (consumible_id) REFERENCES consumibles(id)
);