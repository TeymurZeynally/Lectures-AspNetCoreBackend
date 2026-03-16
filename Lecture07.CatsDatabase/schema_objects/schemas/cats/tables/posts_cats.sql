CREATE TABLE IF NOT EXISTS cats.posts_cats (
    post_id         BIGINT      NOT NULL,
    cat_id          BIGINT      NOT NULL,

    CONSTRAINT pk_posts_cats PRIMARY KEY (post_id, cat_id),
    CONSTRAINT fk_posts_cats_post FOREIGN KEY (post_id) REFERENCES cats.posts(id),
    CONSTRAINT fk_posts_cats_cat FOREIGN KEY (cat_id) REFERENCES cats.cats(id)
);