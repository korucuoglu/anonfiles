CREATE OR REPLACE PROCEDURE public.add_category(IN c_title text, IN c_userid integer)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
INSERT
	INTO
	category (title,
	user_id)
VALUES(c_title,
c_userid);

COMMIT;
END;

$procedure$
;
