CREATE TABLE
    IF NOT EXISTS products
(
    id          UUID PRIMARY KEY,
    name        TEXT             NOT NULL,
    description TEXT             NOT NULL,
    category    TEXT             NOT NULL,
    price       double precision NOT NULL,
    stock       int              NOT NULL
);