// app/ViewModels/DashboardViewModel.php
<?php

namespace App\ViewModels;

use App\Repositories\CmsRepository;
use App\Services\SpaceDataService;

class DashboardViewModel
{
    private CmsRepository $cmsRepository;
    private SpaceDataService $spaceDataService;
    
    public function __construct(
        CmsRepository $cmsRepository,
        SpaceDataService $spaceDataService
    ) {
        $this->cmsRepository = $cmsRepository;
        $this->spaceDataService = $spaceDataService;
    }
    
    public function getCmsContent(string $slug = 'dashboard_experiment'): string
    {
        try {
            return $this->cmsRepository->getContentBySlug($slug) 
                ?: '<div class="text-muted">Блок не найден</div>';
        } catch (\Throwable $e) {
            \Log::error('Ошибка получения CMS блока', [
                'slug' => $slug,
                'error' => $e->getMessage()
            ]);
            
            return '<div class="text-danger">Ошибка загрузки данных</div>';
        }
    }
    
    public function getDashboardData(): array
    {
        return [
            'cms_content' => $this->getCmsContent(),
            'iss_data' => $this->spaceDataService->getIssData(),
            'trend_data' => $this->spaceDataService->getIssTrend(),
            'jwst_data' => $this->spaceDataService->getJwstImages(),
        ];
    }
}