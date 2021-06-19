library(datasets)
library(graphics)
library(ggplot2)
library(gginference)
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
confidence.interval <- as.numeric(args[7])
data[,variable] <- factor(data[,variable])

## Bar plot i pie plot
png(file=paste(path, "barplot.png", sep = "/"))
barplot(table(data[,variable]))
dev.off()
png(file=paste(path, "pieplot.png", sep = "/"))
pie(table(data[,variable]))
dev.off()

## Test o jednoj proporciji
# broj eksperimenata
n = length(data[,variable])
# broj uspjeha
x = length(which(data[,variable] == level))
# egzaktni binomni test o jednoj proporciji
test.result <- binom.test(x, n, p = null.hypothesis, 
                          alternative = alternative.hypothesis,
                          conf.level = confidence.interval)
cat(test.result$statistic, test.result$parameter, 
    test.result$p.value, test.result$conf.int, 
    test.result$estimate, sep = " ")