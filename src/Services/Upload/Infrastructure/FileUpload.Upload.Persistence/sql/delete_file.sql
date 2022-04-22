CREATE OR REPLACE PROCEDURE public.delete_file(IN fileid integer, IN userid integer)
 LANGUAGE plpgsql
AS $procedure$
	
	DECLARE usedspace int;
	BEGIN
		
		SELECT size INTO usedspace  FROM files WHERE files.id = fileid;
	
		DELETE FROM files WHERE files.id = fileid AND files.user_id = userid;
			
		UPDATE userinfo SET used_space=used_space-usedspace  WHERE id = userid;	
	
		COMMIT;
	END;
$procedure$
;
