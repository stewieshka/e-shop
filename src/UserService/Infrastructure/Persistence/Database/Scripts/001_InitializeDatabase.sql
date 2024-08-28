CREATE TABLE
    IF NOT EXISTS users
(
    id            UUID PRIMARY KEY,
    username      TEXT NOT NULL,
    email         TEXT NOT NULL,
    password_salt TEXT NOT NULL,
    password_hash TEXT NOT NULL
);

CREATE TABLE
    IF NOT EXISTS sessions
(
    refresh_token UUID PRIMARY KEY,
    user_id       UUID                        NOT NULL,
    expires_at    timestamp without time zone not null,
    ip            TEXT                        NOT NULL,
    user_agent    TEXT                        NOT NULL,

    CONSTRAINT fk_user
        FOREIGN KEY (user_id) REFERENCES users (id)
            ON DELETE CASCADE
);