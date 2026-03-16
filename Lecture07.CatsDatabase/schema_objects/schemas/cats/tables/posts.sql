CREATE TABLE IF NOT EXISTS cats.posts (
    id              BIGSERIAL       NOT NULL    PRIMARY KEY,
    user_id         BIGINT          NOT NULL,
    uid             UUID            NOT NULL    UNIQUE DEFAULT gen_random_uuid(),
    title           VARCHAR(100)    NOT NULL,
    description     TEXT                NULL,
    photo_url       VARCHAR(255)    NOT NULL,
    created_at      TIMESTAMPTZ     NOT NULL    DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_posts_user FOREIGN KEY (user_id) REFERENCES auth.users(id)
);