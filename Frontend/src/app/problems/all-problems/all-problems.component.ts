import { Component, OnInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ProblemService } from '../../services/problems/problem.service';

@Component({
    selector: 'app-all-problems',
    templateUrl: './all-problems.component.html',
    styleUrls: ['./all-problems.component.css'],
})
export class AllProblemsComponent implements OnInit {
    problemsByYear: { [year: number]: Problem[] } = {};
    filteredProblemsByYear: { [year: number]: Problem[] } = {};
    filteredYears: number[] = [];
    
    years: { item_id: number, item_text: string }[] = [];
    grades: { item_id: number, item_text: string }[] = [];
    
    selectedYears: { item_id: number, item_text: string }[] = [];
    selectedGrades: { item_id: number, item_text: string }[] = [];

    dropdownSettings = {
        singleSelection: false,
        idField: 'item_id',
        textField: 'item_text',
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        allowSearchFilter: true
    };

    constructor(private problemService: ProblemService) {}

    ngOnInit(): void {
        this.problemService.getAllProblems().subscribe((data: Problem[]) => {
            this.groupProblemsByYear(data);
            this.initializeFilters();
        });
    }

    private groupProblemsByYear(problems: Problem[]): void {
        problems.forEach((problem) => {
            const year = problem.year;
            if (!this.problemsByYear[year]) {
                this.problemsByYear[year] = [];
            }
            this.problemsByYear[year].push(problem);
        });
        this.filteredProblemsByYear = { ...this.problemsByYear };
        this.filteredYears = this.getYears();
    }

    private initializeFilters(): void {
        this.years = this.getYears().map(year => ({ item_id: year, item_text: year.toString() }));
        const uniqueGrades = new Set<number>();
        Object.values(this.problemsByYear).forEach(problems => {
            problems.forEach(problem => uniqueGrades.add(problem.grade));
        });
        this.grades = Array.from(uniqueGrades).sort((a, b) => a - b).map(grade => ({ item_id: grade, item_text: grade.toString() }));
    }

    getYears(): number[] {
        return Object.keys(this.problemsByYear)
            .map((year) => parseInt(year))
            .sort((a, b) => b - a);
    }

    filterProblems(): void {
        this.filteredProblemsByYear = {};
        const selectedYearIds = this.selectedYears.map(year => year.item_id);
        const selectedGradeIds = this.selectedGrades.map(grade => grade.item_id);

        const years = this.getYears();

        for (const year of years) {
            if (selectedYearIds.length && !selectedYearIds.includes(year)) {
                continue;
            }

            const problems = this.problemsByYear[year].filter(problem => {
                return !selectedGradeIds.length || selectedGradeIds.includes(problem.grade);
            });

            if (problems.length) {
                this.filteredProblemsByYear[year] = problems;
            }
        }
        this.filteredYears = Object.keys(this.filteredProblemsByYear).map(year => parseInt(year)).sort((a, b) => b - a);
    }
}
