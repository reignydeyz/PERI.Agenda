export class Statistics {
    labels: Array<string>;
    values: Array<number>;
}

export class GraphData {
    data: Array<number>;
    label: string;
}

export class GraphDataSet {
    dataSet: GraphData[];
    chartLabels: string[];
}