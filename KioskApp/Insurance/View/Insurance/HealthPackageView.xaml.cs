using Insurance.ViewModel;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using static Insurance.View.HealthPackageView;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for HealthPackageView.xaml
    /// </summary>
    public partial class HealthPackageView : UserControl
    {
        public HealthPackageView()
        {
            InitializeComponent();

            FetchInsurancePackages();
            FetchPackageDetails(1);
            
        }
        public class InsurancePackage
        {
            public int id { get; set; }
            public string packageName { get; set; }
            public int insuranceType { get; set; }
            public string typeName { get; set; }
            public int duration { get; set; }
            public string payType { get; set; }
            public int fee { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }
        public class InsurancePackageDetail
        {
            public int id { get; set; }
            public string content { get; set; }
            public int coverage { get; set; }
            public string description { get; set; }
            public int packageId { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }
        public class BenefitDetail
        {
            public int id { get; set; }
            public string content { get; set; }
            public int coverage { get; set; }
            public int benefitId { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }
        private async void FetchInsurancePackages()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7017/api/InsurancePackage/ShowInsurancePackage";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        List<InsurancePackage> insurancePackages = JsonConvert.DeserializeObject<List<InsurancePackage>>(jsonResponse);

                        UpdateUI(insurancePackages);
                    }
                    else
                    {
                        // Handle unsuccessful API request
                        // You might want to log or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You might want to log or display an error message
            }
        }

        private void UpdateUI(List<InsurancePackage> insurancePackages) // show package
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            // Assuming StackPanel is the name of your StackPanel in XAML
            StackPanel stackPanel = FindName("StackPanel") as StackPanel;

            if (stackPanel != null)
            {
                for (int i = 0; i < insurancePackages.Count; i++)
                {
                    Button packageButton = new Button
                    {
                        Content = $"Gói {i + 1}: {insurancePackages[i].fee.ToString("C", cultureInfo)}/năm",
                        Style = i == 0 ? (Style)FindResource("PackageButtonSelected") : (Style)FindResource("PackageButton"),
                        Width = 367,
                        Margin = new Thickness(0, 0, 0, 14)
                    };

                    // Add a click event handler for the button if needed
                    packageButton.Click += PackageButton_Click;

                    stackPanel.Children.Add(packageButton);
                }
            }
        }
        
        private async void FetchPackageDetails(int packageId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7017/api/InsurancePackage/ShowInsurancePackageDetail/{packageId}";


                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        List<InsurancePackageDetail> packageDetail = JsonConvert.DeserializeObject<List<InsurancePackageDetail>>(jsonResponse);

                        UpdateBenefitsUI(packageDetail);
                    }
                    else
                    {
                        // Handle unsuccessful API request
                        // You might want to log or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You might want to log or display an error message
            }
        }

        private void UpdateBenefitsUI(List<InsurancePackageDetail> packageDetails) // show benefit
        {
            int id = 1;
            // Assuming stackPanelBenefit is the name of your StackPanel in XAML
            StackPanel stackPanelBenefit = FindName("stackPanelBenefit") as StackPanel;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            if (stackPanelBenefit != null)
            {
                stackPanelBenefit.Children.Clear(); // Clear existing children

                foreach (var packageDetail in packageDetails)
                {
                    StackPanel benefitPanel = new StackPanel { Margin = new Thickness(5) };

                    DockPanel dockPanel = new DockPanel();
                    TextBlock benefitText = new TextBlock
                    {
                        Text = $"{id}. {packageDetail.content}",
                        Foreground = Brushes.Black,
                        FontFamily = new FontFamily("Exo"),
                        FontWeight = FontWeights.Bold,
                        FontSize = 16
                    };

                    TextBlock coverageText = new TextBlock
                    {
                        Text = $"{packageDetail.coverage.ToString("C", cultureInfo)}",
                        Foreground = Brushes.Black,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };

                    TextBlock descriptionText = new TextBlock
                    {
                        Margin = new Thickness(10, 0, 0, 0),
                        TextWrapping = TextWrapping.Wrap,
                        Width = 300,
                        Text = $"{packageDetail.description}",
                        Foreground = Brushes.Black,
                        Padding = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    dockPanel.Children.Add(benefitText);
                    dockPanel.Children.Add(coverageText);

                    benefitPanel.Children.Add(dockPanel);
                    benefitPanel.Children.Add(descriptionText);

                    // Add any additional UI elements for the benefit, such as descriptions, grids, etc.

                    Rectangle separator = new Rectangle { Fill = Brushes.Black, Height = 1, Margin = new Thickness(5, 0, 5, 0) };
                    benefitPanel.Children.Add(separator);

                    stackPanelBenefit.Children.Add(benefitPanel);

                    // Fetch and display benefit details
                    FetchAndDisplayBenefitDetails(packageDetail.id);

                    id++;
                }
            }
        }

        private async void FetchAndDisplayBenefitDetails(int benefitId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7017/api/InsurancePackage/ShowBenefitDetailById/{benefitId}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        List<BenefitDetail> benefitDetails = JsonConvert.DeserializeObject<List<BenefitDetail>>(jsonResponse);

                        UpdateBenefitDetailsUI(benefitDetails);
                    }
                    else
                    {
                        // Handle unsuccessful API request
                        // You might want to log or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You might want to log or display an error message
            }
        }

        private void UpdateBenefitDetailsUI(List<BenefitDetail> benefitDetails)
        {
            // Assuming gridBenefitDetails is the name of your Grid in XAML
            Grid gridBenefitDetails = FindName("gridBenefitDetails") as Grid;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            if (gridBenefitDetails != null)
            {
                // Clear existing children in the grid
                gridBenefitDetails.Children.Clear();

                foreach (var benefitDetail in benefitDetails)
                {
                    // Create a new TextBlock for displaying benefit details
                    TextBlock benefitDetailText = new TextBlock
                    {
                        Margin = new Thickness(10, 0, 0, 0),
                        TextWrapping = TextWrapping.Wrap,
                        Width = 300,
                        Text = $"{benefitDetail.content}: {benefitDetail.coverage.ToString("C", cultureInfo)}",
                        Foreground = Brushes.Black,
                        Padding = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    // Add the benefit detail TextBlock to the grid
                    gridBenefitDetails.Children.Add(benefitDetailText);
                }
            }
        }





        private void PackageButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle button click event if needed
        }

        

    }
}
