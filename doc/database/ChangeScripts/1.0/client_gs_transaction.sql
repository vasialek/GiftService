
CREATE TABLE `gs_transaction` (
  `id` int(11) NOT NULL,
  `pos_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL DEFAULT '0',
  `transaction_status_id` tinyint(4) NOT NULL DEFAULT '0',
  `product_uid` varchar(32) NOT NULL,
  `product_name` varchar(1024) NOT NULL,
  `product_price_minor` int(11) NOT NULL,
  `paid_amount_minor` int(11) NOT NULL DEFAULT '-1',
  `created_at_utc` datetime NOT NULL,
  `response_at_utc` datetime NOT NULL
);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `gs_transaction`
--
ALTER TABLE `gs_transaction`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `product_uid` (`product_uid`),
  ADD KEY `status` (`transaction_status_id`),
  ADD KEY `pos_id` (`pos_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `gs_transaction`

ALTER TABLE `gs_transaction` CHANGE `id` `id` INT(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `gs_transaction` ADD `product_duration` VARCHAR(512) NOT NULL AFTER `product_price_minor`;
