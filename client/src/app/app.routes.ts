import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { SkillsListComponent } from './skills/skills-list/skills-list.component';
import { RegisterComponent } from './register/register.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'skills/skills-list', component: SkillsListComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];
