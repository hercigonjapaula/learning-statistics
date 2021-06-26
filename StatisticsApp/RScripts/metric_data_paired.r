library(datasets)
library(graphics)
library(ggplot2)
library(gginference)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = args[1]

## Skup podataka
data <- read.table(as.character(args[2]), header = TRUE) 
alternative.hypothesis <- as.character(args[3])
confidence.interval <- as.numeric(args[4])

## Histogrami
png(file=paste(path, "histogram1.png", sep = "/"))
hist(data[,1], main = paste(names(data[,1]), "histogram", sep = " "), 
     xlab = "Prvo mjerenje", ylab = "Frekvencija")
dev.off()
png(file=paste(path, "histogram2.png", sep = "/"))
hist(data[,2], main = paste(names(data[,2]), "histogram", sep = " "),
     xlab = "Drugo mjerenje", ylab = "Frekvencija")
dev.off()

## Test za uparene podatke
test.result <- t.test(data[,1], data[,2], paired = TRUE, 
                      alternative = alternative.hypothesis,
                      conf.level = confidence.interval)
cat(test.result$statistic, test.result$parameter, 
    test.result$p.value, test.result$conf.int, 
    test.result$estimate, sep = " ")  
ggttest(test.result, colaccept="lightsteelblue1", 
        colreject="grey84", colstat="navyblue")
ggsave("test_plot.png", path = path)