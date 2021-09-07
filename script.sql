CREATE DATABASE pantry
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

\c pantry

CREATE TABLE IF NOT EXISTS public."Account"
(
    "acc_ID" uuid NOT NULL,
    name "char" NOT NULL,
    email "char" NOT NULL,
    password character varying(64) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Account_pkey" PRIMARY KEY ("acc_ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Account"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Products"
(
    "item_ID" "char" NOT NULL,
    quantity "char" NOT NULL,
    category "char" NOT NULL,
    price "char" NOT NULL,
    name "char" NOT NULL,
    CONSTRAINT "Products_pkey" PRIMARY KEY ("item_ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Products"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipes"
(
    "recipe_ID" integer NOT NULL,
    steps "char" NOT NULL,
    recipe_name "char" NOT NULL,
    CONSTRAINT "Recipes_pkey" PRIMARY KEY ("recipe_ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Recipes"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Inventory_List"
(
    "item_ID" "char" NOT NULL,
    "acc_ID" uuid NOT NULL,
    "duplicate_ID" uuid NOT NULL,
    exp_date date NOT NULL,
    CONSTRAINT "Inventory_List_pkey" PRIMARY KEY ("item_ID", "acc_ID", "duplicate_ID"),
    CONSTRAINT "Inventory_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Inventory_List_item_ID_fkey" FOREIGN KEY ("item_ID")
        REFERENCES public."Products" ("item_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Inventory_List"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipe_List"
(
    "recipe_ID" integer NOT NULL,
    "item_ID" "char" NOT NULL,
    "acc_ID" uuid NOT NULL,
    quantity "char" NOT NULL,
    CONSTRAINT "Recipe_List_pkey" PRIMARY KEY ("recipe_ID"),
    CONSTRAINT "Recipe_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Recipe_List_item_ID_fkey" FOREIGN KEY ("item_ID")
        REFERENCES public."Products" ("item_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Recipe_List_recipe_ID_fkey" FOREIGN KEY ("recipe_ID")
        REFERENCES public."Recipes" ("recipe_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Recipe_List"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Shopping_List"
(
    "item_ID" "char" NOT NULL,
    "acc_ID" uuid NOT NULL,
    count integer NOT NULL,
    CONSTRAINT "Shopping_List_pkey" PRIMARY KEY ("item_ID", "acc_ID"),
    CONSTRAINT "Shopping_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Shopping_List_item_ID_fkey" FOREIGN KEY ("item_ID")
        REFERENCES public."Products" ("item_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Shopping_List"
    OWNER to postgres;