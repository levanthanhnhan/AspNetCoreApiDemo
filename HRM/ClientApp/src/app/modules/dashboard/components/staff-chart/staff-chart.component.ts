import { Component, OnDestroy } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartFontOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { DashboardService } from '../../dashboard.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'staff-chart',
  templateUrl: './staff-chart.component.html',
  styleUrls: ['./staff-chart.component.css']
})

export class StaffChartComponent implements OnDestroy {

  subscription: Subscription;

  constructor(private dashBoardService: DashboardService) { }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    const self = this;

    this.subscription = this.dashBoardService.getData().subscribe(res => {
      const _arrYear = []
      const _arrCount = []
      const result = res[0];

      Object.keys(result).map(function (idx) {
        _arrYear.push(result[idx].year);
        _arrCount.push(result[idx].count);
      });

      self.chartData = [
        { data: _arrCount, label: 'Staff' },
      ];
      self.chartLabels = _arrYear;
    });
  }

  getRefreshToken(): string {
    return localStorage.getItem('refreshToken');
  }

  chartData: ChartDataSets[] = [
    { data: [], label: 'Staff' },
  ];

  chartLabels: Label[] = [];

  chartOptions: ChartOptions = {
    responsive: true,
    showLines: true,
  };

  chartFontOptions: ChartFontOptions = {
    defaultFontFamily: 'Nunito, Segoe UI, Roboto, Helvetica Neue, Arial,sans-serif'
  };

  chartColors: Color[] = [
    {
      borderColor: 'rgba(78, 115, 223, 1)',
      backgroundColor: 'rgba(78, 115, 223, 0.05)',
    },
  ];

  chartLegend = false;
  chartPlugins = [];
  chartType = 'line';
}
