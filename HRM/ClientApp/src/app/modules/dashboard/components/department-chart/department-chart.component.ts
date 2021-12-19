import { Component, Inject } from '@angular/core';
import { ChartType } from 'chart.js';
import { Label, MultiDataSet } from 'ng2-charts';
import { DashboardService } from '../../dashboard.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'department-chart',
  templateUrl: './department-chart.component.html',
  styleUrls: ['./department-chart.component.css']
})

export class DepartmentChartComponent {

  subscription: Subscription;

  constructor(private dashBoardService: DashboardService) { }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    const self = this;

    this.subscription = this.dashBoardService.getData().subscribe(res => {
      const _arrDepartmentName = []
      const _arrMembers = []
      const _arrLeaderName = []
      const _arrColor = []
      const result = res[1];

      Object.keys(result).map(function (idx) {
        _arrDepartmentName.push(result[idx].departmentName);
        _arrMembers.push(result[idx].members);
        _arrLeaderName.push(result[idx].leaderName);
        _arrColor.push(self.randomColor());
      });

      self.chartData = _arrMembers;
      self.chartLabels = _arrDepartmentName;
      self.chartColors[0].backgroundColor = _arrColor;
    });
  }

  chartLabels: Label[] = [];

  chartData: MultiDataSet = [
    []
  ];

  chartType: ChartType = 'doughnut';

  chartColors = [
    {
      backgroundColor: []
    }
  ];

  randomColor() {
    return '#' + Math.floor(Math.random() * 16777215).toString(16);
  }
}
