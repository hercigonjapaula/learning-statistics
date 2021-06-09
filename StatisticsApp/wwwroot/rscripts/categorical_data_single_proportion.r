library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/"

## Skup podataka
data <- read.table(as.character(args[1]),sep=";",header = TRUE)
variable <- as.numeric(args[2])
null.hypothesis <- as.numeric(args[3])
alternative.hypothesis <- as.character(args[4])
level <- as.character(args[5])

## Bar plot i pie plot
png(file=paste(path, "test_plots/barplot.png", sep = ""))
barplot(table(data[,variable]))
dev.off()
png(file=paste(path, "test_plots/pieplot.png", sep = ""))
pie(table(data[,variable]))
dev.off()

## Test o jednoj proporciji
# broj eksperimenata
n = length(data[,variable])
# broj "uspjeha"
x = length(which(data[,variable] == level))
# egzaktni binomni test o jednoj proporciji
binom.test(x, n, p = null.hypothesis, 
           alternative = alternative.hypothesis)