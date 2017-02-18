drop  procedure if exists elmah_GetErrorXml;

CREATE PROCEDURE `elmah_GetErrorXml` (
  IN Id CHAR(36),
  IN App VARCHAR(60)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    SELECT  `AllXml`
    FROM    `elmah_error`
    WHERE   `ErrorId` = Id AND `Application` = App;
END;