library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)

## Putanja
path = args[1]

## Skup podataka
data <- read.table(as.character(args[2]),sep=";",header = TRUE)
variable <- as.numeric(args[3])
null.hypothesis <- as.numeric(args[4])
alternative.hypothesis <- as.character(args[5])
level <- as.character(args[6])
data[,variable] <- factor(data[,variable])

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
# broj uspjeha
x = length(which(data[,variable] == level))
# egzaktni binomni test o jednoj proporciji
binom.test(x, n, p = null.hypothesis, 
           alternative = alternative.hypothesis)