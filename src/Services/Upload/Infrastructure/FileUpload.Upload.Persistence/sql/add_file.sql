CREATE OR REPLACE PROCEDURE public.add_file
(IN filename text, IN filesize bigint, IN filekey text, IN fileuser_id integer, 
IN categoryids integer[] DEFAULT '{}'::integer[])
 LANGUAGE plpgsql
AS $procedure$

DECLARE file_id int;
DECLARE _categoryid int;
	BEGIN
		
		INSERT INTO files (file_name, "size", file_key, user_id) 
			values(filename, filesize, filekey, fileuser_id) RETURNING id INTO file_id;
			
		UPDATE userinfo SET used_space=used_space+filesize WHERE id = fileuser_id;
		
		FOR _categoryid IN SELECT unnest(categoryids)
	     LOOP
		     IF EXISTS (SELECT 1 FROM categories c WHERE c.id = _categoryid AND user_id = fileuser_id) THEN 
			  	INSERT INTO filecategory (category_id, file_id) VALUES (_categoryid, file_id);
			  ELSE
			  	CONTINUE;
			 END IF;
	     END LOOP;
		COMMIT;
	END;
$procedure$
;


CALL add_file('1.txt', 50, '11', 1, '{1,4}');
			