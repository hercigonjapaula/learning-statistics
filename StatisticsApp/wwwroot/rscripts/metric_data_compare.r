library(datasets)
library(graphics)
library(ggplot2)
library(stats)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/"

## Skup podataka
data1 <- scan(as.character(args[1]))
data2 <- scan(as.character(args[2]))
alternative.hypothesis <- as.character(args[3])
confidence.interval <- as.numeric(args[4])
test <- as.character(args[5])

## Box plot 
png(file=paste(path, "test_plots/boxplot.png", sep = ""))
boxplot(data1, data2, main = "Pravokutni dijagrami obje populacije")
dev.off()

## Test o jednakosti srednjih vrijednosti / varijanci dviju populacija
switch(test,
       "mean" = t.test(data1, data2, 
                       alternative = alternative.hypothesis,
                       conf.level = 0.95,
                       var.equal = TRUE),
       "var" = var.test(data1, data2, 
                        alternative = alternative.hypothesis,
                        conf.level = 0.95)
)