ALTER TABLE `w_actual`
ADD COLUMN `description` VARCHAR(500) NULL DEFAULT NULL AFTER `item_vcd`;

SPGetActualInfo : them a.description